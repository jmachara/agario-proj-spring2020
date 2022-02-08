using Microsoft.Extensions.Logging;
using Model;
using NetworkingNS;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
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
///    
/// </summary>
namespace ViewController
{
    public partial class GameView : Form
    {
        private World world;
        private int OnWindowCircleRadius = 20;
        private int WorldHeightAndWidth = 5000;
        private int GameWindowHeightAndWidth = 800;
        private bool WorldObjectsRecieved = false;


        private string PlayerName;
        private Circle playerCircle;
        private List<Circle> playerSubCircles = new List<Circle>();

        private string ServerAddressName { get; set; }
        private Preserved_Socket_State server;
        private ILogger logger;

        public GameView(ILogger logger)
        {
            this.logger = logger;
            this.world = new World(WorldHeightAndWidth, WorldHeightAndWidth);
            InitializeMenuComponents();
        }
        /// <summary>
        /// When the connect button is clicked the game gets the server address and player name and tries to connect to the server
        /// while initilizing the game screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            string potentialPlayerName = PlayerNameBox.Text;
            if (ServerAddressBox.Text.Length == 0)
                this.ServerAddressName = "localhost";
            else
                this.ServerAddressName = ServerAddressBox.Text;

            if (potentialPlayerName.Length == 0)
                this.PlayerName = "NoName";
            else
                this.PlayerName = potentialPlayerName;

            ConnectToServer(e);
            DisableMenuEnableGame();
        }
        /// <summary>
        /// Clears the forms controls to create space to draw on the form and adds the labels about the game. 
        /// </summary>
        private void DisableMenuEnableGame()
        {
            this.Controls.Clear();
            this.Controls.Add(TotalFoodInGameLabel);
            this.Controls.Add(PlayerWorldPositionLabel);
            this.Controls.Add(CursorWorldPositionLabel);
            this.Controls.Add(PlayerMassLabel);
        }
        /// <summary>
        /// Tries to connect to the server
        /// </summary>
        /// <param name="e"></param>
        public void ConnectToServer(EventArgs e)
        {
            try
            {
                if (this.server != null && this.server.socket.Connected)
                {
                    this.server.socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                    return;
                }
                this.server = Networking.Connect_to_Server(Contact_Established, ServerAddressName);
            }
            catch (Exception exception)
            {
                logger.LogError($"Couldn't connect to server : {exception}");

            }
        }
        /// <summary>
        /// When the contact is established we get ready to recieve the players circle object.
        /// </summary>
        /// <param name="obj"></param>
        private void Contact_Established(Preserved_Socket_State obj)
        {
            try
            {
            logger.LogDebug("Connection established");
            obj.on_data_received_handler = RecievePlayerCircle;
            Networking.Send(obj.socket, PlayerName);
            Networking.await_more_data(obj);
            }
            catch(Exception exception)
            {
                logger.LogError($"Error connecting to server {exception}");
            }

        }
        /// <summary>
        /// Recieves the player circle that the server sends
        /// </summary>
        /// <param name="obj"></param>
        private void RecievePlayerCircle(Preserved_Socket_State obj)
        {
            try
            {
                Circle playerCircle = JsonConvert.DeserializeObject<Circle>(obj.Message);
                this.playerCircle = playerCircle;
                world.circleList.Add(playerCircle.idNumber, playerCircle);
                obj.on_data_received_handler = RecieveAndUpdateWorldObjects;
                Networking.await_more_data(obj);
            }
            catch (Exception exception)
            {
                logger.LogError($"Could Not recieve player circle: {exception}");
            }
        }

        /// <summary>
        /// Called once all of the starting circles are recieved. Recieves updated information about the world.
        /// </summary>
        /// <param name="obj"></param>
        private void RecieveAndUpdateWorldObjects(Preserved_Socket_State obj)
        {
            try
            {
                JObject recievedCircle = JObject.Parse(obj.Message);
                int circleType = (int)recievedCircle["type"];
                //if the recieved circle is a food or player
                if (circleType == 0 || circleType == 1)
                {
                    Circle worldCircle = JsonConvert.DeserializeObject<Circle>(obj.Message);
                    lock (world.circleList)
                    {
                        UpdateCircleList(worldCircle);
                    }
                }
                else if (circleType == 2)
                {
                    if (!WorldObjectsRecieved)
                        WorldObjectsRecieved = true;
                    this.Invalidate();
                    SendMoveRequest(obj);
                }
                else if (circleType == 3)
                {
                    //ToDo
                }

            }
            catch (Exception exception)
            {
                logger.LogError($"Failed to update circle list : {exception}");
            }
            if (!obj.Has_More_Data())
            {
                Networking.await_more_data(obj);
            }

        }
        /// <summary>
        /// Sends a move request to the server
        /// </summary>
        /// <param name="obj"></param>
        private void SendMoveRequest(Preserved_Socket_State obj)
        {
            getMousePositionInWorld(out int mousePositionInWorldX, out int mousePositionInWorldY);
            Networking.Send(obj.socket, $"(move,{mousePositionInWorldX},{mousePositionInWorldY})");

        }

        private void getMousePositionInWorld(out int mousePositionInWorldX, out int mousePositionInWorldY)
        {
            double worldToWindowRatio = playerCircle.radius / this.OnWindowCircleRadius;
            int playerCircleScreenLocationX = this.Location.X + (GameWindowHeightAndWidth / 2);
            int playerCircleScreenLocationY = this.Location.Y + (GameWindowHeightAndWidth / 2);
            mousePositionInWorldX = (int)(worldToWindowRatio * (MousePosition.X - playerCircleScreenLocationX));
            mousePositionInWorldY = (int)(worldToWindowRatio * (MousePosition.Y - playerCircleScreenLocationY));
            
        }
        /// <summary>
        /// Updates the list of circles in the world 
        /// </summary>
        /// <param name="recievedCircle"></param>
        private void UpdateCircleList(Circle recievedCircle)
        {
            if (world.circleList.ContainsKey(recievedCircle.idNumber))
            {
                world.circleList.Remove(recievedCircle.idNumber);
                if(recievedCircle.Mass > 0)
                {
                    world.circleList.Add(recievedCircle.idNumber, recievedCircle);
                    if(recievedCircle.idNumber == playerCircle.idNumber)
                    {
                        playerCircle = recievedCircle;
                    }
                }
                else
                {
                    if(recievedCircle.Type == 0)
                    {
                        world.NumberOfFoodCircles--;
                    }
                    else
                    {
                        if(recievedCircle.idNumber == playerCircle.idNumber)
                        { 
                            if(playerSubCircles.Count == 0)
                            {
                                logger.LogDebug("Player died, closing game");
                                this.Close();
                                
                            }
                            else
                            {
                                this.playerCircle = playerSubCircles[0];
                                playerSubCircles.RemoveAt(0);
                            }
                        }
                        world.NumberOfPlayerCircles--;
                    }
                }
            }
            else
            {
                if (recievedCircle.Mass > 0)
                {
                    world.circleList.Add(recievedCircle.idNumber, recievedCircle);
                    if(recievedCircle.PlayerId == playerCircle.idNumber)
                    {
                        this.playerSubCircles.Add(recievedCircle);
                    }
                    if (recievedCircle.Type == 0)
                    {
                        world.NumberOfFoodCircles++;
                    }
                    else
                    {
                        world.NumberOfPlayerCircles++;
                    }
                }
            }

        }
        /// <summary>
        /// Draws the current state of the world on the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Draw_Scene(object sender, PaintEventArgs e)
        {
            if (WorldObjectsRecieved)
            {
                lock (world.circleList)
                {
                    e.Graphics.Clear(Color.White);
                    TotalFoodInGameLabel.Text = $"Food : {world.NumberOfFoodCircles}";
                    PlayerWorldPositionLabel.Text = $"Player Position : {playerCircle.position.X} , {playerCircle.position.Y}";
                    CursorWorldPositionLabel.Text = $"MouseScreenlocation : {MousePosition.X} , {MousePosition.Y}";
                    PlayerMassLabel.Text = $"Player Mass : {playerCircle.Mass}";
                    foreach (KeyValuePair<int, Circle> entry in world.circleList)
                    {
                        if (IsOnScreen(entry.Value))
                        {
                            DrawCircle(entry.Value, e);
                        }
                    }
                }
            }

        }
        /// <summary>
        /// Determines if the circle is on the screen.
        /// </summary>
        /// <param name="circle"></param>
        /// <returns></returns>
        private bool IsOnScreen(Circle circle)
        {
            double worldToWindowRatio = playerCircle.radius / this.OnWindowCircleRadius;
            double currentCircleX = circle.position.X;
            double currentCircleY = circle.position.Y;
            bool onScreenX = Math.Abs(currentCircleX - playerCircle.position.X) < ((GameWindowHeightAndWidth * worldToWindowRatio) / 2);
            bool onScreenY = Math.Abs(currentCircleY - playerCircle.position.Y) < ((GameWindowHeightAndWidth * worldToWindowRatio) / 2);
            return onScreenX && onScreenY;

        }
        /// <summary>
        /// Draws the circle on the screen
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="e"></param>
        private void DrawCircle(Circle circle, PaintEventArgs e)
        {
            double worldToWindowRatio = playerCircle.radius / this.OnWindowCircleRadius;
            Brush brush;
            //Food is brown and players are green due to server sending all 0's for colors.
            if (circle.Type == 0)
                brush = new SolidBrush(Color.SaddleBrown);
            else
                brush = new SolidBrush(Color.ForestGreen);
            Pen pen = new Pen(brush);
            Point drawingRectanglePoint;
            Point namePoint;
            if (circle.idNumber == playerCircle.idNumber)
            {
                drawingRectanglePoint = new Point((GameWindowHeightAndWidth / 2) - this.OnWindowCircleRadius, (GameWindowHeightAndWidth / 2) - this.OnWindowCircleRadius);
                namePoint = new Point((GameWindowHeightAndWidth / 2), (GameWindowHeightAndWidth / 2) - (this.OnWindowCircleRadius + 12));
            }
            else
            {
                double screenXZeroWorldCoord = playerCircle.position.X - ((GameWindowHeightAndWidth / 2) * worldToWindowRatio);
                double screenYZeroWorldCoord = playerCircle.position.Y - ((GameWindowHeightAndWidth / 2) * worldToWindowRatio);
                double newPointX = ((circle.position.X - screenXZeroWorldCoord) / worldToWindowRatio) - (circle.radius / worldToWindowRatio);
                double newPointY = ((circle.position.Y - screenYZeroWorldCoord) / worldToWindowRatio) - (circle.radius / worldToWindowRatio);
                namePoint = new Point((int)((circle.position.X - screenXZeroWorldCoord) / worldToWindowRatio), (int)newPointY - 12);
                drawingRectanglePoint = new Point((int)newPointX, (int)newPointY);
            }
            Rectangle rectForCircleDrawing = new Rectangle(drawingRectanglePoint, new Size((int)((circle.radius * 2) / worldToWindowRatio), (int)((circle.radius * 2) / worldToWindowRatio)));
            e.Graphics.DrawEllipse(pen, rectForCircleDrawing);
            e.Graphics.FillEllipse(brush, rectForCircleDrawing);
            if (circle.Type == 1)
            {
                Brush wordBrush = new SolidBrush(Color.Black);
                Font wordFont = new Font("Arial", 12);
                e.Graphics.DrawString(circle.PlayerName, wordFont, wordBrush, namePoint);
                wordFont.Dispose();
                wordBrush.Dispose();
            }
            brush.Dispose();
            pen.Dispose();
        }
        /// <summary>
        /// Sends the command to split the circle to the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendSplit(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Space)
            {
                getMousePositionInWorld(out int mousePositionInWorldX, out int mousePositionInWorldY);
                Networking.Send(server.socket, $"(split,{mousePositionInWorldX},{mousePositionInWorldY})");
                logger.LogDebug("logger sent split command");
            }
        }
    }

}
