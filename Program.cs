using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsLinuxDemo;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainForm());
    }
}

public class MainForm : Form
{
    private Button myButton;
    private Label myLabel;

    public MainForm()
    {
        // Window Configuration
        this.Text = "Ubuntu 24.04 - WinForms Demo";
        this.Size = new Size(400, 250);
        this.StartPosition = FormStartPosition.CenterScreen;

        // Label Initialization
        myLabel = new Label
        {
            Text = "Hello from WinForms on Wayland!",
            Location = new Point(30, 30),
            AutoSize = true,
            Font = new Font("Sans", 11, FontStyle.Bold)
        };

        // Button Initialization
        myButton = new Button
        {
            Text = "Click Me",
            Location = new Point(30, 80),
            Size = new Size(120, 40)
        };
        
        myButton.Click += MyButton_Click;

        // Add controls to Form
        this.Controls.Add(myLabel);
        this.Controls.Add(myButton);
    }

    private void MyButton_Click(object? sender, EventArgs e)
    {
        myLabel.Text = $"Clicked at: {DateTime.Now:HH:mm:ss}";
        MessageBox.Show("WinForms event handlers are working as expected!", "Event Triggered");
    }
}
