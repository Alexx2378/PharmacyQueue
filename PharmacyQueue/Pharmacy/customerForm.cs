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

        // Array for easier access to Now Serving labels
        private Label[] nowServingLabels;

        public customerForm(Home adminForm)
        {
            InitializeComponent();
            this.adminFormInstance = adminForm;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Initialize the array for Now Serving labels
            nowServingLabels = new Label[] { null, nowServctmr1, nowServctmr2, nowServctmr3 };
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
            customerNum.Text = "Number";
            for (int i = 1; i <= 3; i++)
            {
                UpdateNowServingLabel(i, nowServingLabels[i]?.Text ?? "");
            }
        }

        private void AddToShortestQueue(string queueNumber, bool isPriority)
        {
            if (counter1Queue.Count >= MAX_QUEUE_SIZE &&
                counter2Queue.Count >= MAX_QUEUE_SIZE &&
                counter3Queue.Count >= MAX_QUEUE_SIZE)
            {
                MessageBox.Show("All counters are currently full. Please wait.", "Queue Full",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<string> targetQueue = GetShortestQueue();

            if (isPriority)
                targetQueue.Insert(0, queueNumber);
            else
                targetQueue.Add(queueNumber);

            UpdateAdminForm();
        }

        private List<string> GetShortestQueue()
        {
            int[] counts = { counter1Queue.Count, counter2Queue.Count, counter3Queue.Count };
            List<List<string>> queues = new List<List<string>> { counter1Queue, counter2Queue, counter3Queue };

            int minIndex = -1;
            int minCount = int.MaxValue;
            for (int i = 0; i < 3; i++)
            {
                if (counts[i] < minCount && counts[i] < MAX_QUEUE_SIZE)
                {
                    minCount = counts[i];
                    minIndex = i;
                }
            }
            return queues[minIndex];
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
            if (this.IsDisposed || !this.IsHandleCreated)
                return;

            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateNowServingLabel(counter, number)));
                return;
            }

            if (counter < 1 || counter > 3) return;

            var label = nowServingLabels[counter];
            if (label != null && label.Text != number)
            {
                label.Text = number;
                HighlightNowServing(counter);
            }
        }

        private void customerNum_Click(object sender, EventArgs e) { }

        private void label11_Click(object sender, EventArgs e) { }

        private void customerForm_Load(object sender, EventArgs e)
        {
            customerNum.Text = "Number";
            if (adminFormInstance != null && !adminFormInstance.IsDisposed)
            {
                nowServctmr1.Text = adminFormInstance.nowServ1.Text;
                nowServctmr2.Text = adminFormInstance.nowServ2.Text;
                nowServctmr3.Text = adminFormInstance.nowServ3.Text;
            }
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void nowServctmr1_Click(object sender, EventArgs e) { }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (adminFormInstance != null && !adminFormInstance.IsDisposed)
                {
                    e.Cancel = true;
                    this.Hide();
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        public void HighlightNowServing(int counter)
        {
            if (counter < 1 || counter > 3) return;
            var control = nowServingLabels[counter];
            if (control == null) return;

            Timer blinkTimer = new Timer();
            int blinkCount = 0;
            blinkTimer.Interval = 500;
            blinkTimer.Tick += (s, args) =>
            {
                if (blinkCount == 5)
                {
                    control.BackColor = SystemColors.Control;
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

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

