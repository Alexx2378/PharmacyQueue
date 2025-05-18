using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Media;

namespace PharmacyQueue
{
    public partial class Home : Form
    {
        private SpeechSynthesizer synthesizer = new SpeechSynthesizer();
        public Home()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Center the form on the screen
        }

        // Add this method to your Form1.cs class
        private List<string> GetListBoxItems(ListBox listBox)
        {
            List<string> items = new List<string>();
            foreach (var item in listBox.Items)
            {
                items.Add(item.ToString());
            }
            return items;
        }

        // Method to update queue displays, called from customerForm
        public void UpdateQueueDisplays(List<string> counter1Queue, List<string> counter2Queue, List<string> counter3Queue)
        {
            // Update Counter 1 display
            UpdateCounterDisplay(counter1ListBox, counter1Queue);
            
            // Update Counter 2 display
            UpdateCounterDisplay(counter2ListBox, counter2Queue);
            
            // Update Counter 3 display
            UpdateCounterDisplay(counter3ListBox, counter3Queue);
        }

        private void UpdateCounterDisplay(ListBox counterListBox, List<string> queue)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateCounterDisplay(counterListBox, queue)));
                return;
            }

            // Clear the current items in the ListBox
            counterListBox.Items.Clear();
            
            // Add each queue number as a separate item in the ListBox
            foreach (string number in queue)
            {
                counterListBox.Items.Add(number);
            }
        }

        // Method to serve the next customer (remove from queue)
        public void ServeNextCustomer(int counterNumber)
        {
            // This method would be called when a "Next" button is clicked for a specific counter
            // It would remove the first customer from the queue and update the display
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void counter1panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void counter2panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void counter1ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Ding sound method
        private void PlayDingSound()
        {
            try
            {
                // Try multiple possible locations for the sound file
                string fileName = "ding-47489.wav";
                string soundPath = null;
                
                // Possible locations to check
                string[] possiblePaths = {
                    // Direct in application folder
                    System.IO.Path.Combine(Application.StartupPath, fileName),
                    // In Pharmacy subfolder
                    System.IO.Path.Combine(Application.StartupPath, "Pharmacy", fileName),
                    // Relative to current directory
                    fileName,
                    // In parent directory
                    System.IO.Path.Combine("..", fileName)
                };
                
                // Find the first path that exists
                foreach (string path in possiblePaths)
                {
                    if (System.IO.File.Exists(path))
                    {
                        soundPath = path;
                        break;
                    }
                }
                
                // If we found a valid path, play the sound
                if (!string.IsNullOrEmpty(soundPath))
                {
                    SoundPlayer player = new SoundPlayer(soundPath);
                    player.PlaySync(); // This will block until the sound finishes playing
                }
                else
                {
                    // If no valid path was found, show an error
                    MessageBox.Show("Could not find sound file in any of the expected locations.", 
                        "Sound Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                MessageBox.Show("Error playing sound: " + ex.Message, 
                    "Sound Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Call this method in your takeOrderC1_Click, takeOrderC2_Click, and takeOrderC3_Click_1 methods
        private void takeOrderC1_Click(object sender, EventArgs e)
        {
            if (this.IsDisposed || !this.IsHandleCreated)
            {
                return;
            }

            // First check if there's a current order being processed
            if (!string.IsNullOrWhiteSpace(number1.Text) && number1.Text != "No customers" && number1.Text != "Number")
            {
                // There's already an order in progress that hasn't been completed
                MessageBox.Show("Please complete the current order first by clicking 'Complete Order'.",
                    "Order In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if there are any items in the counter1ListBox
            if (counter1ListBox.Items.Count > 0)
            {
                // Get the first item (next customer) from the list
                string nextCustomer = counter1ListBox.Items[0].ToString();
                
                // Display the customer number in the number1 label
                number1.Text = nextCustomer;
                
                // Update both Now Serving labels
                nowServ1.Text = nextCustomer;
                PlayDingSound();
                SpeakNowServing(nextCustomer, 1);
                HighlightNowServing(1); // <-- Add this line here
                
                // Remove the customer from the list
                counter1ListBox.Items.RemoveAt(0);
                
                // Update the static queue in customerForm
                customerForm.UpdateCounter1Queue(GetListBoxItems(counter1ListBox));
                
                // Update the customer form's Now Serving label through the instance
                if (customerFormInstance != null && !customerFormInstance.IsDisposed && customerFormInstance.IsHandleCreated)
                {
                    customerFormInstance.UpdateNowServingLabel(1, nextCustomer);
                    customerFormInstance.HighlightNowServing(1); // <-- Add this line
                }
            }
            else
            {
                // If there are no customers in the queue, display a message
                number1.Text = "No customers";
                MessageBox.Show("No customers in Counter 1 queue.", "Queue Empty", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void takeOrderC2_Click(object sender, EventArgs e)
        {
            if (this.IsDisposed || !this.IsHandleCreated)
            {
                return;
            }

            // First check if there's a current order being processed
            if (!string.IsNullOrWhiteSpace(number2.Text) && number2.Text != "No customers" && number2.Text != "Number")
            {
                // There's already an order in progress that hasn't been completed
                MessageBox.Show("Please complete the current order first by clicking 'Complete Order'.", 
                    "Order In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        
            // Check if there are any items in the counter2ListBox
            if (counter2ListBox.Items.Count > 0)
            {
                string nextCustomer = counter2ListBox.Items[0].ToString();
                
                number2.Text = nextCustomer;
                nowServ2.Text = nextCustomer;
                PlayDingSound();
                SpeakNowServing(nextCustomer, 2);
                HighlightNowServing(2); // <-- Add this line here
                
                counter2ListBox.Items.RemoveAt(0);
                customerForm.UpdateCounter2Queue(GetListBoxItems(counter2ListBox));
                
                // Update using the instance
                if (customerFormInstance != null && !customerFormInstance.IsDisposed)
                {
                    customerFormInstance.UpdateNowServingLabel(2, nextCustomer);
                }
            }
            else
            {
                // If there are no customers in the queue, display a message
                number2.Text = "No customers";
                MessageBox.Show("No customers in Counter 2 queue.", "Queue Empty", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Add a class-level field to store the customer form instance
        private customerForm customerFormInstance;
        
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if we have an existing customer form instance
                if (customerFormInstance == null || customerFormInstance.IsDisposed)
                {
                    // Create a new customer form only if one doesn't exist
                    customerFormInstance = new customerForm(this);
                    
                    // Subscribe to the form's FormClosing event
                    customerFormInstance.FormClosing += (s, args) =>
                    {
                        if (args.CloseReason == CloseReason.UserClosing)
                        {
                            args.Cancel = true;  // Prevent the form from actually closing
                            customerFormInstance.Hide();  // Hide it instead
                            this.Show();  // Show the admin form
                        }
                    };
                }
                
                // Update the displays before showing
                customerFormInstance.RefreshDisplays();
                
                // Show the customer form and hide this form
                customerFormInstance.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error switching to customer form: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void takeOrderC3_Click_1(object sender, EventArgs e)
        {
            if (this.IsDisposed || !this.IsHandleCreated)
            {
                return;
            }

            // First check if there's a current order being processed
            if (!string.IsNullOrWhiteSpace(number3.Text) && number3.Text != "No customers" && number3.Text != "Number")
            {
                // There's already an order in progress that hasn't been completed
                MessageBox.Show("Please complete the current order first by clicking 'Complete Order'.", 
                    "Order In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        
            // Check if there are any items in the counter3ListBox
            if (counter3ListBox.Items.Count > 0)
            {
                string nextCustomer = counter3ListBox.Items[0].ToString();
                
                number3.Text = nextCustomer;
                nowServ3.Text = nextCustomer;
                PlayDingSound();
                SpeakNowServing(nextCustomer, 3);
                HighlightNowServing(3); // <-- Add this line here
                
                counter3ListBox.Items.RemoveAt(0);
                customerForm.UpdateCounter3Queue(GetListBoxItems(counter3ListBox));
                
                // Update using the instance
                if (customerFormInstance != null && !customerFormInstance.IsDisposed)
                {
                    customerFormInstance.UpdateNowServingLabel(3, nextCustomer);
                }
            }
            else
            {
                // If there are no customers in the queue, display a message
                number3.Text = "No customers";
                MessageBox.Show("No customers in Counter 3 queue.", "Queue Empty",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void compOrderC1_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(number1.Text) && number1.Text != "No customers")
            {
                // Complete the order and clear the display
                MessageBox.Show("Order for " + number1.Text + " has been completed.",
                    "Order Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Clear the number display to indicate no active order
                number1.Text = "";
                
                // Clear the Now Serving label
                nowServ1.Text = "";
                
                // Update the customer form's Now Serving label
                if (customerFormInstance != null && !customerFormInstance.IsDisposed)
                {
                    customerFormInstance.UpdateNowServingLabel(1, "");
                }
            }
            else
            {
                // No order to complete
                MessageBox.Show("No active order to complete.", "No Order",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void HighlightNowServing(int counter)
        {
            Control control = null;
            switch (counter)
            {
                case 1:
                    control = nowServ1;
                    break;
                case 2:
                    control = nowServ2;
                    break;
                case 3:
                    control = nowServ3;
                    break;
            }

            if (control != null)
            {
                Timer blinkTimer = new Timer();
                int blinkCount = 0;

                blinkTimer.Interval = 500; // Set the interval to 500 milliseconds
                blinkTimer.Tick += (s, args) =>
                {
                    if (blinkCount == 5)
                    {
                        control.BackColor = SystemColors.Control; // Reset to default color
                        blinkTimer.Stop();
                        blinkTimer.Dispose();
                    }
                    else
                    {
                        control.BackColor = control.BackColor == Color.Yellow ? SystemColors.Control : Color.Yellow;
                        blinkCount++;
                    }
                };

                blinkTimer.Start();
            }
        }

        private void SpeakNowServing(string number, int counter)
        {
            synthesizer.SpeakAsync($"Now serving. number {number}, please proceed to counter {counter}");
        }


        private void compOrderC2_Click_1(object sender, EventArgs e)
        {
            // Check if there's an order to complete
            if (!string.IsNullOrEmpty(number2.Text) && number2.Text != "No customers")
            {
                // Complete the order and clear the display
                MessageBox.Show("Order for " + number2.Text + " has been completed.",
                    "Order Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear the number display to indicate no active order
                number2.Text = "";

                // Clear the Now Serving label for Counter 2 (fixed from nowServ1 to nowServ2)
                nowServ2.Text = "";
            }
            else
            {
                // No order to complete
                MessageBox.Show("No active order to complete.", "No Order",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void compOrderC3_Click_1(object sender, EventArgs e)
        {
            // Check if there's an order to complete
            if (!string.IsNullOrEmpty(number3.Text) && number3.Text != "No customers")
            {
                // Complete the order and clear the display
                MessageBox.Show("Order for " + number3.Text + " has been completed.",
                    "Order Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear the number display to indicate no active order
                number3.Text = "";

                // Clear the Now Serving label for Counter 3 (fixed from nowServ1 to nowServ3)
                nowServ3.Text = "";
            }
            else
            {
                // No order to complete
                MessageBox.Show("No active order to complete.", "No Order",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }
        private void label11_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}



