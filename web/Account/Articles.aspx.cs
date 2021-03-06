﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public partial class Account_News : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            image_thumb.Visible = false;
          
            FillGrid();
           
        }
    }


    private void clear()
    {
        txt_Title.Text = "";
        image_thumb.Visible = false;
        txt_msg.InnerText = "";
    }

    private void FillGrid()
    {
        try
        {
            DataTable Grid_dt = new DB().ADMIN_Articles_Get();

            grid_Display.DataSource = Grid_dt;
            grid_Display.DataBind();


        }
        catch (Exception ex)
        {

            lbl_error.Text = ex.Message;
        }
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
          
            string thumb_url = null;
            if (lbl_id.Text == "")
            {
                if ( upload_Thumb.HasFile)
                {
               Random rand = new Random((int)DateTime.Now.Ticks);
            int numIterations = 0;
            numIterations = rand.Next(1, 10000);


            upload_Thumb.SaveAs(Server.MapPath(@"~/css/Articles/" + numIterations + upload_Thumb.FileName));
                     thumb_url = (@"~/css/Articles/" + numIterations + upload_Thumb.FileName);


                     new DB().Admin_Articles_Insert(txt_Title.Text, txt_msg.InnerText,thumb_url);

                    FillGrid();
                    clear();
                    lbl_error.Text = "saved successfully";
                    lbl_error.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lbl_error.Text = "Please Choose Image ";
                }
            }
            /////////////////
            else
            {
                bool valid = true;
                if (upload_Thumb.HasFile)
                {
                    if (valid)
                    {
                        Random rand = new Random((int)DateTime.Now.Ticks);
                        int numIterations = 0;
                        numIterations = rand.Next(1, 10000);




                        upload_Thumb.SaveAs(Server.MapPath(@"~/css/Articles/" + numIterations + upload_Thumb.FileName));
                        thumb_url = (@"~/css/Articles/" + numIterations + upload_Thumb.FileName);
                    }
                }
                else
                {
                    DataTable dt = new DB().ADMIN_Articles_GetByID(int.Parse(lbl_id.Text));
                    
                    thumb_url = dt.Rows[0]["Thumb"].ToString();
                }
                if (valid)
                {

                    new DB().Admin_Articles_Update(int.Parse(lbl_id.Text), txt_Title.Text, txt_msg.InnerText,thumb_url);

                    DataTable edit_dt = new DB().ADMIN_Articles_GetByID(int.Parse(lbl_id.Text));
                    Bind(edit_dt);
                    FillGrid();
                    lbl_id.Text = "";
                    image_thumb.Visible = false;
                 
                    lbl_error.Text = "saved successfully";
                    Response.Redirect(Request.RawUrl);
                }

            }

        }
        catch (SqlException sql)
        {
            lbl_error.Text = sql.Message;

        }

        catch (Exception ex)
        {
            lbl_error.Text = ex.Message;
        }
    }

    protected void grid_Display_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grid_Display.PageIndex = e.NewPageIndex;
        FillGrid();

    }

    protected void grid_Display_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int id = (int)grid_Display.DataKeys[e.RowIndex].Value;
            new DB().ADMIN_Articles_Delete(id);
            FillGrid();
        }
        catch (Exception ex)
        {

            lbl_error.Text = ex.Message;
        }
    }

    protected void grid_Display_RowEditing(object sender, GridViewEditEventArgs e)
    {
        image_thumb.Visible = true;
       
        int id = (int)grid_Display.DataKeys[e.NewEditIndex].Value;
        DataTable edit_dt = new DB().ADMIN_Articles_GetByID(id);
        Bind(edit_dt);
        lbl_id.Text = id.ToString();
    }

    private void Bind(DataTable edit_dt)
    {
        txt_Title.Text = edit_dt.Rows[0]["Title"].ToString();
        txt_msg.InnerText = edit_dt.Rows[0]["Body"].ToString();
        image_thumb.ImageUrl = edit_dt.Rows[0]["Img"].ToString();
       

    }

    protected void grid_Display_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton btn_del = (LinkButton)e.Row.FindControl("btn_delete");

            btn_del.Attributes.Add("onclick", "javascript:return " +
                "confirm('Are you sure') ");

        }
    }
}