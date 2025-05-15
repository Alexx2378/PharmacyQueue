// ... existing code ...
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
        
        // Remove the customer from the list
        counter1ListBox.Items.RemoveAt(0);
        
        // Update the static queue in customerForm
        customerForm.UpdateCounter1Queue(GetListBoxItems(counter1ListBox));
        
        // Update the customer form's Now Serving label through the instance
        if (customerFormInstance != null && !customerFormInstance.IsDisposed && customerFormInstance.IsHandleCreated)
        {
            customerFormInstance.UpdateNowServingLabel(1, nextCustomer);
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
// ... existing code ...