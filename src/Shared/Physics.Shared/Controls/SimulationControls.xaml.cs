﻿using Physics.Shared.UI.Helpers;
using System.Numerics;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Physics.Shared.UI.Controls
{
    public sealed partial class SimulationControls : UserControl
    {
        public SimulationControls()
        {
            this.InitializeComponent();
            StepSizeNumberBox.SetupFormatting();
            StepSizeForwardNumberBox.SetupFormatting();
			PreviewKeyDown += SimulationControls_KeyDown;
        }

		private void SimulationControls_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
		{
			if (e.Key == Windows.System.VirtualKey.Space)
			{
				e.Handled = true;
				if (IsPaused)
				{
					PlayCommand?.Execute(null);
				}
				else
				{
					PauseCommand?.Execute(null);
				}
			}
		}

		public ICommand PlayCommand
        {
            get => (ICommand)GetValue(PlayCommandProperty);
            set => SetValue(PlayCommandProperty, value);
        }

        public static readonly DependencyProperty PlayCommandProperty =
            DependencyProperty.Register(nameof(PlayCommand), typeof(ICommand), typeof(SimulationControls), new PropertyMetadata(null));

        public ICommand PauseCommand
        {
            get => (ICommand)GetValue(PauseCommandProperty);
            set => SetValue(PauseCommandProperty, value);
        }

        public static readonly DependencyProperty PauseCommandProperty =
            DependencyProperty.Register(nameof(PauseCommand), typeof(ICommand), typeof(SimulationControls), new PropertyMetadata(null));

        public ICommand JumpBackCommand
        {
            get => (ICommand)GetValue(JumpBackCommandProperty);
            set => SetValue(JumpBackCommandProperty, value);
        }

        public static readonly DependencyProperty JumpBackCommandProperty =
            DependencyProperty.Register(nameof(JumpBackCommand), typeof(ICommand), typeof(SimulationControls), new PropertyMetadata(null));

        public ICommand JumpForwardCommand
        {
            get => (ICommand)GetValue(JumpForwardCommandProperty);
            set => SetValue(JumpForwardCommandProperty, value);
        }

        public static readonly DependencyProperty JumpForwardCommandProperty =
            DependencyProperty.Register(nameof(JumpForwardCommand), typeof(ICommand), typeof(SimulationControls), new PropertyMetadata(null));

        public ICommand JumpToStartCommand
        {
            get => (ICommand)GetValue(JumpToStartCommandProperty);
            set => SetValue(JumpToStartCommandProperty, value);
        }

        public static readonly DependencyProperty JumpToStartCommandProperty =
            DependencyProperty.Register(nameof(JumpToStartCommand), typeof(ICommand), typeof(SimulationControls), new PropertyMetadata(null));

        public ICommand JumpToEndCommand
        {
            get => (ICommand)GetValue(JumpToEndCommandProperty);
            set => SetValue(JumpToEndCommandProperty, value);
        }

        public static readonly DependencyProperty JumpToEndCommandProperty =
            DependencyProperty.Register(nameof(JumpToEndCommand), typeof(ICommand), typeof(SimulationControls), new PropertyMetadata(null));

        public ICommand ShareCommand
        {
            get => (ICommand)GetValue(ShareCommandProperty);
            set => SetValue(ShareCommandProperty, value);
        }

        public static readonly DependencyProperty ShareCommandProperty =
            DependencyProperty.Register(nameof(ShareCommand), typeof(ICommand), typeof(SimulationControls), new PropertyMetadata(null));

        public bool IsTimeSliderVisible
        {
            get { return (bool)GetValue(IsTimeSliderVisibleProperty); }
            set { SetValue(IsTimeSliderVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsTimeSliderVisibleProperty =
            DependencyProperty.Register(nameof(IsTimeSliderVisible), typeof(bool), typeof(SimulationControls), new PropertyMetadata(true));

        public double SimulationSpeed
        {
            get { return (double)GetValue(SimulationSpeedProperty); }
            set { SetValue(SimulationSpeedProperty, value); }
        }

        public static readonly DependencyProperty SimulationSpeedProperty =
            DependencyProperty.Register(nameof(SimulationSpeed), typeof(double), typeof(SimulationControls), new PropertyMetadata(1.0));

        public bool IsPaused
        {
            get { return (bool)GetValue(IsPausedProperty); }
            set { SetValue(IsPausedProperty, value); }
        }

        public static readonly DependencyProperty IsPausedProperty =
            DependencyProperty.Register(nameof(IsPaused), typeof(bool), typeof(SimulationControls), new PropertyMetadata(false));

        public double JumpSize
        {
            get { return (double)GetValue(JumpSizeProperty); }
            set { SetValue(JumpSizeProperty, value); }
        }

        public static readonly DependencyProperty JumpSizeProperty =
            DependencyProperty.Register(nameof(JumpSize), typeof(double), typeof(SimulationControls), new PropertyMetadata(0.5));

		public Visibility ForwardVisibility
		{
			get { return (Visibility)GetValue(ForwardVisibilityProperty); }
			set { SetValue(ForwardVisibilityProperty, value); }
		}

		public static readonly DependencyProperty ForwardVisibilityProperty =
			DependencyProperty.Register("ForwardVisibility", typeof(Visibility), typeof(SimulationControls), new PropertyMetadata(Visibility.Visible));

		public Visibility BackwardVisibility
		{
			get { return (Visibility)GetValue(BackwardVisibilityProperty); }
			set { SetValue(BackwardVisibilityProperty, value); }
		}

		public static readonly DependencyProperty BackwardVisibilityProperty =
			DependencyProperty.Register("BackwardVisibility", typeof(Visibility), typeof(SimulationControls), new PropertyMetadata(Visibility.Visible));

		public Visibility SpeedControlVisibility
		{
			get { return (Visibility)GetValue(SpeedControlVisibilityProperty); }
			set { SetValue(SpeedControlVisibilityProperty, value); }
		}

		public static readonly DependencyProperty SpeedControlVisibilityProperty =
			DependencyProperty.Register("SpeedControlVisibility", typeof(Visibility), typeof(SimulationControls), new PropertyMetadata(Visibility.Visible));

		private void ToggleSpeedSlider()
        {
            if (SpeedSldr.Opacity == 0)
            {
                SpeedButtonStoryboardShow.Begin();
            }
            else
            {
                SpeedButtonStoryboardHide.Begin();
            }
        }
	}
}
