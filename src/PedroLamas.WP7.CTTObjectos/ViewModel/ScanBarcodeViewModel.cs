using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Media;
using Cimbalino.Phone.Toolkit.Services;
using com.google.zxing;
using com.google.zxing.common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Phone.Controls;
using PedroLamas.WP7.CTTObjectos.Message;

namespace PedroLamas.WP7.CTTObjectos.ViewModel
{
    public class ScanBarcodeViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IVibrationService _vibrationService;
        private readonly IPhotoCameraService _photoCameraService;

        private readonly Reader _barcodeReader;
        private Thread _scanBarcodeThread;
        private Timer _focusCameraTimer;
        private volatile bool _scanning;
        private VideoBrush _videoPreviewBrush;
        private PageOrientation _orientation = PageOrientation.None;

        #region Properties

        public VideoBrush VideoPreviewBrush
        {
            get
            {
                return _videoPreviewBrush;
            }
            private set
            {
                if (_videoPreviewBrush == value)
                    return;

                _videoPreviewBrush = value;

                RaisePropertyChanged(() => VideoPreviewBrush);
            }
        }

        public RelayCommand<OrientationChangedEventArgs> OrientationChangedCommand { get; set; }

        #endregion

        public ScanBarcodeViewModel(INavigationService navigationService, IVibrationService vibrationService, IPhotoCameraService photoCameraService)
        {
            _navigationService = navigationService;
            _vibrationService = vibrationService;
            _photoCameraService = photoCameraService;

            _photoCameraService.Initialized += (s, e) =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    _photoCameraService.FlashMode = Microsoft.Devices.FlashMode.Off;

                    _scanning = true;

                    _scanBarcodeThread = new Thread(ScanBarcode);
                    _scanBarcodeThread.Start();

                    _focusCameraTimer = new Timer(FocusCamera, null, 1000, 4000);
                });
            };

            OrientationChangedCommand = new RelayCommand<OrientationChangedEventArgs>(e =>
            {
                _orientation = e.Orientation;

                CheckVideoPreviewOrientation(_videoPreviewBrush);
            });

            _barcodeReader = new com.google.zxing.oned.Code39Reader(false);
        }

        public void StartScanning()
        {
            _photoCameraService.Start();

            var _videoPreviewBrush = _photoCameraService.CreateVideoBrush();

            CheckVideoPreviewOrientation(_videoPreviewBrush);

            VideoPreviewBrush = _videoPreviewBrush;
        }

        public void StopScanning()
        {
            _scanning = false;

            if (_focusCameraTimer != null)
            {
                _focusCameraTimer.Dispose();
                _focusCameraTimer = null;
            }

            if (_scanBarcodeThread != null)
            {
                _scanBarcodeThread.Join(200);
                _scanBarcodeThread = null;
            }

            VideoPreviewBrush = null;

            if (_photoCameraService != null)
            {
                _photoCameraService.Stop();
            }
        }

        private void ScanBarcode()
        {
            var zxingHints = new Dictionary<object, object>() { { DecodeHintType.TRY_HARDER, true } };

            var cameraPreviewBuffer = new byte[640 * 480];
            var cameraPreviewSampleBuffer = new byte[640 * 120];

            Thread.Sleep(300);

            while (_scanning)
            {
                _photoCameraService.GetPreviewBufferY(cameraPreviewBuffer);

                Array.Copy(cameraPreviewBuffer, 640 * 180, cameraPreviewSampleBuffer, 0, 640 * 120);

                Result readerResult = null;

                try
                {
                    var luminance = new RGBLuminanceSource(cameraPreviewSampleBuffer, 640, 120, true);
                    var binarizer = new HybridBinarizer(luminance);
                    var binaryBitmap = new BinaryBitmap(binarizer);

                    readerResult = _barcodeReader.decode(binaryBitmap, zxingHints);
                }
                catch
                {
                }

                if (readerResult != null)
                {
                    _vibrationService.Start();

                    _scanning = false;

                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        MessengerInstance.Send<ScannedBarcodeMessage>(new ScannedBarcodeMessage(readerResult.Text));

                        _navigationService.GoBack();
                    });
                }
            }
        }

        private void FocusCamera(object state)
        {
            if (!_scanning)
                return;

            _photoCameraService.Focus();
        }

        private void CheckVideoPreviewOrientation(VideoBrush videoPreviewBrush)
        {
            if (videoPreviewBrush == null)
                return;

            if (_orientation == PageOrientation.LandscapeRight)
            {
                videoPreviewBrush.RelativeTransform = new CompositeTransform() { CenterX = 0.5, CenterY = 0.5, Rotation = 180 };
            }
            else
            {
                videoPreviewBrush.RelativeTransform = new CompositeTransform() { CenterX = 0.5, CenterY = 0.5, Rotation = 0 };
            }
        }
    }
}