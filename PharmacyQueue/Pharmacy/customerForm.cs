using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PharmacyQueue
{
    public partial class customerForm : Form
    {
        // Static variables to keep track of queue numbers
        private static int regularNumber = 0;
        private static int priorityNumber = 0;
        
        // Static lists to store queue data for each counter
        private static List<string> counter1Queue = new List<string>();
        private static List<string> counter2Queue = new List<string>();
        private static List<string> counter3Queue = new List<string>();
        
        // Reference to the admin form and constants
        private readonly Home adminFormInstance;
        private const int MAX_QUEUE_SIZE = 10;

        public customerForm(Home adminForm)
        {
            InitializeComponent();
            this.adminFormInstance = adminForm;
            this.StartPosition = FormStartPosition.CenterScreen; 
        }

        // Regular queue button click handler
        private void regularbttn_Click(object sender, EventArgs e)
        {
            GenerateQueueNumber(false);
        }

        // Priority queue button click handler
        private void prioritybttn_Click(object sender, EventArgs e)
        {
            GenerateQueueNumber(true);
        }

        private void GenerateQueueNumber(bool isPriority)
        {
            int number = isPriority ? ++priorityNumber : ++regularNumber;
            string prefix = isPriority ? "P-" : "R-";
            string queueNumber = $"{prefix}{number:D3}";
            
            customerNum.Text = queueNumber;
            AddToShortestQueue(queueNumber, isPriority);
        }

        public void RefreshDisplays()
        {
            // Reset the customer number display
            customerNum.Text = "Number";
            
            // Update any Now Serving displays if needed
            UpdateNowServingLabel(1, nowServctmr1?.Text ?? "");
            UpdateNowServingLabel(2, nowServctmr2?.Text ?? "");
            UpdateNowServingLabel(3, nowServctmr3?.Text ?? "");
        }

        private void AddToShortestQueue(string queueNumber, bool isPriority)
        {
            int counter1Count = counter1Queue.Count;
            int counter2Count = counter2Queue.Count;
            int counter3Count = counter3Queue.Count;
            
            if (counter1Count >= MAX_QUEUE_SIZE && counter2Count >= MAX_QUEUE_SIZE && counter3Count >= MAX_QUEUE_SIZE)
            {
                MessageBox.Show("All counters are currently full. Please wait.", "Queue Full", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            List<string> targetQueue = GetShortestQueue();
            
            if (isPriority)
            {
                targetQueue.Insert(0, queueNumber);
            }
            else
            {
                targetQueue.Add(queueNumber);
            }
            
            UpdateAdminForm();
        }

        private List<string> GetShortestQueue()
        {
            int counter1Count = counter1Queue.Count;
            int counter2Count = counter2Queue.Count;
            int counter3Count = counter3Queue.Count;
            
            if (counter1Count <= counter2Count && counter1Count <= counter3Count && counter1Count < MAX_QUEUE_SIZE)
            {
                return counter1Queue;
            }
            else if (counter2Count <= counter3Count && counter2Count < MAX_QUEUE_SIZE)
            {
                return counter2Queue;
            }
            else
            {
                return counter3Queue;
            }
        }

        private async void UpdateAdminForm()
        {
            adminFormInstance?.UpdateQueueDisplays(counter1Queue, counter2Queue, counter3Queue);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateAdminForm();
            adminFormInstance.Show();
            this.Hide();
        }
        
        public static void UpdateCounter1Queue(List<string> newQueue)
        {
            counter1Queue = new List<string>(newQueue);
        }
        
        public static void UpdateCounter2Queue(List<string> newQueue)
        {
            counter2Queue = new List<string>(newQueue);
        }
        
        public static void UpdateCounter3Queue(List<string> newQueue)
        {
            counter3Queue = new List<string>(newQueue);
        }
        
        public void UpdateNowServingLabel(int counter, string number)
        {
            // Check if the form is disposed or handle is not created
            if (this.IsDisposed || !this.IsHandleCreated)
            {
                return;
            }
        
            // Ensure UI updates happen on the UI thread
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateNowServingLabel(counter, number)));
                return;
            }
        
            switch(counter)
            {
                case 1:
                    nowServctmr1.Text = number;
                    if (!string.IsNullOrEmpty(number)) HighlightNowServing(1);
                    break;
                case 2:
                    nowServctmr2.Text = number;
                    if (!string.IsNullOrEmpty(number)) HighlightNowServing(2);
                    break;
                case 3:
                    nowServctmr3.Text = number;
                    if (!string.IsNullOrEmpty(number)) HighlightNowServing(3);
                    break;
            }
        }

        private void customerNum_Click(object sender, EventArgs e)
        {
            // This event handler is required by the designer
            // You can leave it empty if no specific action is needed
        }

        private void label11_Click(object sender, EventArgs e)
        {
            // This event handler is required by the designer
            // You can leave it empty if no specific action is needed
        }

        private void customerForm_Load(object sender, EventArgs e)
        {
            // Initialize any required form settings or data
            customerNum.Text = "Number";  // Set default text

            // Remove the queue clearing code to preserve existing queue data
            // counter1Queue.Clear();
            // counter2Queue.Clear();
            // counter3Queue.Clear();

            // Synchronize Now Serving labels with admin form if available
            if (adminFormInstance != null && !adminFormInstance.IsDisposed)
            {
                nowServctmr1.Text = adminFormInstance.nowServ1.Text;
                nowServctmr2.Text = adminFormInstance.nowServ2.Text;
                nowServctmr3.Text = adminFormInstance.nowServ3.Text;
            }

            // Set the form to start at the center of the screen
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void nowServctmr1_Click(object sender, EventArgs e)
        {
            // This event handler is required by the designer
            // You can leave it empty if no specific action is needed
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // If the admin form is still open, just hide this form
                if (adminFormInstance != null && !adminFormInstance.IsDisposed)
                {
                    e.Cancel = true;  // Cancel the closing
                    this.Hide();      // Hide instead
                }
                else
                {
                    // If admin form is closed, allow this form to close
                    Application.Exit();
                }
            }
        }

        public void HighlightNowServing(int counter)
        {
            Control control = null;
            switch (counter)
            {
                case 1:
                    control = nowServctmr1;
                    break;
                case 2:
                    control = nowServctmr2;
                    break;
                case 3:
                    control = nowServctmr3;
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


    }
}

