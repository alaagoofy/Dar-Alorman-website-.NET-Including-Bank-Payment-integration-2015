﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;


public partial class test_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Random rand = new Random((int)DateTime.Now.Ticks);
        //int numIterations = 0;
        //numIterations = rand.Next(1, 10000);
        //vpc_MerchTxnRef.Text = numIterations.ToString();
        vpc_MerchTxnRef.Text = (Donor.GetMaxId()+1).ToString();
        vpc_OrderInfo.Text = (Donor.GetMaxId() + 1).ToString();
    }
}