using FileLogger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Windows.Forms;
/// <summary> 
/// Author:    Jack Machara
/// Partner:   None
/// Date:      04/8/20
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Jack Machara - This work may not be copied for use in Academic Coursework. 
/// 
/// I, Jack Machara, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// File Contents 
/// 
///    Starts the program and makes a logger for the gameview
/// </summary>
namespace ViewController
{
    static class GameRunner
    {

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ServiceCollection services = new ServiceCollection();

            using (CustomFileLogProvider provider = new CustomFileLogProvider())
            {
                services.AddLogging(configure =>
                {
                    configure.AddProvider(provider);
                    configure.SetMinimumLevel(LogLevel.Debug);
                });

                using (ServiceProvider serviceProvider = services.BuildServiceProvider())
                {
                    ILogger<GameView> logger = serviceProvider.GetRequiredService<ILogger<GameView>>();
                    logger.LogInformation("Logger is working");
                    Application.SetHighDpiMode(HighDpiMode.SystemAware);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    GameView gameView = new GameView(logger);
                    Application.Run(gameView);
                    gameView.Dispose();
                }


            }
        }
    }
}
