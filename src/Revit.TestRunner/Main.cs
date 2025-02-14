﻿using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;
using Revit.TestRunner.Commands;

namespace Revit.TestRunner
{
    public class Main : IExternalApplication
    {
        public Result OnStartup( UIControlledApplication application )
        {
            Log.Info( $"Revit.TestRunner started '{DateTime.Now}'" );
            Log.Info( $"{Environment.OSVersion}, NetFX {Environment.Version}" );
            Log.Debug( $"Log Directory '{Log.LogDirectory}'" );
            Log.Debug( $"CurrentAppDomain.ApplicationBase '{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}'" );

            RibbonPanel ribbonPanel = application.CreateRibbonPanel( "Testing" );

            string command = typeof( TestRunnerCommand ).FullName;

            PushButtonData buttonData = new PushButtonData( command, "Open Runner", Assembly.GetExecutingAssembly().Location, command ) {
                ToolTip = "Open the Test Runner Dialog\nStart Tests using the Revit API.",
                Image = GetImage( "Testing.png", 16 ),
                LargeImage = GetImage( "Testing.png", 32 ),
                AvailabilityClassName = typeof( AvailableInStartScreen ).FullName
            };

            ribbonPanel.AddItem( buttonData );

            return Result.Succeeded;
        }

        public Result OnShutdown( UIControlledApplication application )
        {
            return Result.Succeeded;
        }

        private static ImageSource GetImage( string name, int size )
        {
            ImageSource result = null;

            try {
                string ressource = "pack://application:,,,/Revit.TestRunner;component/View/Pic/" + name;
                result = new BitmapImage( new Uri( ressource ) );
                result = Thumbnail( result, size );
            }
            catch {
                // ignore
            }

            return result;
        }

        private static ImageSource Thumbnail( ImageSource source, int size )
        {
            Rect rect = new Rect( 0, 0, size, size );
            DrawingVisual drawingVisual = new DrawingVisual();
            using( DrawingContext drawingContext = drawingVisual.RenderOpen() ) {
                drawingContext.DrawImage( source, rect );
            }

            RenderTargetBitmap resizedImage = new RenderTargetBitmap( (int)rect.Width, (int)rect.Height, 96, 96, PixelFormats.Default );
            resizedImage.Render( drawingVisual );

            return resizedImage;
        }
    }
}
