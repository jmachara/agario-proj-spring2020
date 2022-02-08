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
///    World class keeps track of the size of the world along with all of the players. It also stores the players circle and the number of players 
///    playing.
/// </summary>
namespace ViewController
{
    partial class GameView
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeMenuComponents()
        {
            this.PlayerMassLabel = new System.Windows.Forms.Label();
            this.PlayerNameBox = new System.Windows.Forms.TextBox();
            this.PlayerNameLabel = new System.Windows.Forms.Label();
            this.ServerAddressBox = new System.Windows.Forms.TextBox();
            this.ServerAddressLabel = new System.Windows.Forms.Label();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.TotalFoodInGameLabel = new System.Windows.Forms.Label();
            this.PlayerWorldPositionLabel = new System.Windows.Forms.Label();
            this.CursorWorldPositionLabel = new System.Windows.Forms.Label();
            // 
            // PlayerNameBox
            // 
            this.PlayerNameBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayerNameBox.Location = new System.Drawing.Point(200, 200);
            this.PlayerNameBox.Name = "PlayerNameBox";
            this.PlayerNameBox.Size = new System.Drawing.Size(300, 100);
            this.PlayerNameBox.TabIndex = 0;
            this.PlayerNameBox.Tag = "";
            // 
            // PlayerNameLabel
            // 
            this.PlayerNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayerNameLabel.AutoSize = true;
            this.PlayerNameLabel.Location = new System.Drawing.Point(200,100);
            this.PlayerNameLabel.Name = "PlayerNameLabel";
            this.PlayerNameLabel.Size = new System.Drawing.Size(300, 100);
            this.PlayerNameLabel.TabIndex = 1;
            this.PlayerNameLabel.Text = "PlayerName";
            // 
            // ServerAddressBox
            // 
            this.ServerAddressBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerAddressBox.Location = new System.Drawing.Point(200, 400);
            this.ServerAddressBox.Name = "ServerAddressBox";
            this.ServerAddressBox.Size = new System.Drawing.Size(300, 100);
            this.ServerAddressBox.TabIndex = 0;
            this.ServerAddressBox.Text = "localhost";
            this.ServerAddressBox.Tag = "";
            // 
            // ServerAddressLabel
            // 
            this.ServerAddressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerAddressLabel.AutoSize = true;    
            this.ServerAddressLabel.Location = new System.Drawing.Point(200, 300);
            this.ServerAddressLabel.Name = "ServerAddressLabel";
            this.ServerAddressLabel.Size = new System.Drawing.Size(300, 100);
            this.ServerAddressLabel.TabIndex = 1;
            this.ServerAddressLabel.Text = "ServerAddress";
            
            //
            //OhNoSpaghett.io
            //
            this.components = new System.ComponentModel.Container();
            this.ClientSize = new System.Drawing.Size(1200,800);
            this.Text = "OhNoSpaghett.io";
            this.BackColor = Color.White;
            this.DoubleBuffered = true;
            this.Paint += new PaintEventHandler(Draw_Scene);
            this.Controls.Add(this.PlayerNameBox);
            this.Controls.Add(this.PlayerNameLabel);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.ServerAddressBox);
            this.Controls.Add(this.ServerAddressLabel);
            this.KeyPress += SendSplit;
            //
            //ConnectButton
            //
            this.ConnectButton.Location = new System.Drawing.Point(200, 500);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(300, 100);
            this.ConnectButton.TabIndex = 4;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // TotalFoodInGameLabel
            // 
            this.TotalFoodInGameLabel.AutoSize = true;
            this.TotalFoodInGameLabel.Location = new System.Drawing.Point(850, 50);
            this.TotalFoodInGameLabel.Name = "TotalFoodInGameLabel";
            this.TotalFoodInGameLabel.Size = new System.Drawing.Size(46, 17);
            this.TotalFoodInGameLabel.TabIndex = 1;
            this.TotalFoodInGameLabel.Text = "";
            // 
            // PlayerWorldPositionLabel
            // 
            this.PlayerWorldPositionLabel.AutoSize = true;
            this.PlayerWorldPositionLabel.Location = new System.Drawing.Point(850, 67);
            this.PlayerWorldPositionLabel.Name = "PlayerWorldPositionLabel";
            this.PlayerWorldPositionLabel.Size = new System.Drawing.Size(46, 17);
            this.PlayerWorldPositionLabel.TabIndex = 1;
            this.PlayerWorldPositionLabel.Text = "";
            // 
            // CursorWorldPositionLabel
            // 
            this.CursorWorldPositionLabel.AutoSize = true;
            this.CursorWorldPositionLabel.Location = new System.Drawing.Point(850, 84);
            this.CursorWorldPositionLabel.Name = "CursorWorldPositionLabel";
            this.CursorWorldPositionLabel.Size = new System.Drawing.Size(46, 17);
            this.CursorWorldPositionLabel.TabIndex = 1;
            this.CursorWorldPositionLabel.Text = "";
            // 
            // PlayerMassLabel
            // 
            this.PlayerMassLabel.AutoSize = true;
            this.PlayerMassLabel.Location = new System.Drawing.Point(850, 101);
            this.PlayerMassLabel.Name = "PlayerMassLabel";
            this.PlayerMassLabel.Size = new System.Drawing.Size(46, 17);
            this.PlayerMassLabel.TabIndex = 1;
            this.PlayerMassLabel.Text = "";


        }



        #endregion
        private System.Windows.Forms.TextBox PlayerNameBox;
        private System.Windows.Forms.Label PlayerMassLabel;
        private System.Windows.Forms.Label PlayerNameLabel;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.TextBox ServerAddressBox;
        private System.Windows.Forms.Label ServerAddressLabel;
        private System.Windows.Forms.Label TotalFoodInGameLabel;
        private System.Windows.Forms.Label PlayerWorldPositionLabel;
        private System.Windows.Forms.Label CursorWorldPositionLabel;

    }
}

