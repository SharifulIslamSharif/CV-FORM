using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;


namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        Image file;
        SqlConnection con = null;
        SqlCommand cmd = null;
        SqlDataAdapter adapt = null;
        //ID variable used in Updating and Deleting Record  
        public static int CvID = 0;

        public Form1()
        {
            InitializeComponent();

            con = new SqlConnection("Data Source=DESKTOP-SV5OLUK;Initial Catalog=CVDetais;Integrated Security=True");
            AddButtonColumn();
            LoadCVs();
            Reset();
        }

        private void LoadCVs()
        {
            con.Open();
            DataTable dt = new DataTable();
            string query = @"SELECT [CvID]
                  ,[Name]
                  ,[FathersName]
                  ,[MothersName]
                  ,[DoB]
                  ,[Gender]
                  ,[Address]
                  ,[Phone]
                  ,[Email]
                  ,[HonoursYear]
                  ,[HonoursBoard]
                  ,[HonoursResult]
                  ,[HonoursGroup]
                  ,[MastersYear]
                  ,[MastersBoard]
                  ,[MastersResult]
                  ,[MastersGroup]
                  ,[Organization1]
                  ,[Organization1Time]
                  ,[Organization2]
                  ,[Organization2Time]
                  ,[Referee1Name]
                  ,[Referee1Address]
                  ,[Referee1Email]
                  ,[Referee2Name]
                  ,[Referee2Address]
                  ,[Referee2Email]
                  ,[FilePath]
              FROM [CV]";
            adapt = new SqlDataAdapter(query, con);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void AddButtonColumn()
        {
            DataGridViewButtonColumn btnReport = new DataGridViewButtonColumn();
            btnReport.HeaderText = "#";
            btnReport.Text = "Report";
            btnReport.Name = "btnReport";
            btnReport.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(btnReport);

            DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
            btnEdit.HeaderText = "#";
            btnEdit.Text = "Edit";
            btnEdit.Name = "btnEdit";
            btnEdit.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(btnEdit);

            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
            btnDelete.HeaderText = "#";
            btnDelete.Text = "Delete";
            btnDelete.Name = "btnDelete";
            btnDelete.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(btnDelete);
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1 && dataGridView1.Rows.Count > e.RowIndex + 1)
            {
                var v = dataGridView1.Rows[e.RowIndex].Cells["CvID"].Value;
                CvID = dataGridView1.Rows[e.RowIndex].Cells["CvID"].Value == null ? 0 : Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["CvID"].Value);
                if ("Report" == dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                {
                    Form2 f2 = new Form2();
                    f2.Show();
                }
                if ("Edit" == dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                {
                    Edit();
                }
                if ("Delete" == dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                {
                    Delete();
                }
            }

        }
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (CvID > 0)
            {
                Updates();
            }
            else
            {
                AddNew();
            }
            Reset();
            LoadCVs();
        }


        private void AddNew()
        {
            // save image
            string strFilePath = AddFile();

            string query = @"INSERT INTO [dbo].[CV]
                  ([Name]
                  ,[FathersName]
                  ,[MothersName]
                  ,[DoB]
                  ,[Gender]
                  ,[Address]
                  ,[Phone]
                  ,[Email]
                  ,[HonoursYear]
                  ,[HonoursBoard]
                  ,[HonoursResult]
                  ,[HonoursGroup]
                  ,[MastersYear]
                  ,[MastersBoard]
                  ,[MastersResult]
                  ,[MastersGroup]
                  ,[Organization1]
                  ,[Organization1Time]
                  ,[Organization2]
                  ,[Organization2Time]
                  ,[Referee1Name]
                  ,[Referee1Address]
                  ,[Referee1Email]
                  ,[Referee2Name]
                  ,[Referee2Address]
                  ,[Referee2Email]

               ,[FilePath])
                VALUES
               (@Name
               ,@FathersName
               ,@MothersName
               ,@DoB
               ,@Gender
               ,@Address
               ,@Phone
               ,@Email
               ,@HonoursYear
               ,@HonoursBoard
               ,@HonoursResult
               ,@HonoursGroup
               ,@MastersYear
               ,@MastersBoard
               ,@MastersResult
               ,@MastersGroup
               ,@Organization1
               ,@Organization1Time
               ,@Organization2
               ,@Organization2Time
               ,@Referee1Name
               ,@Referee1Address
               ,@Referee1Email
               ,@Referee2Name
               ,@Referee2Address
               ,@Referee2Email
               ,@FilePath)";
            cmd = new SqlCommand(query, con);
            con.Open();
            // personal info
            cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@FathersName", txtFName.Text.Trim());
            cmd.Parameters.AddWithValue("@MothersName", txtMName.Text.Trim());
            cmd.Parameters.AddWithValue("@DoB", pickDoB.Value);
            cmd.Parameters.AddWithValue("@Gender", radioFemale.Checked == true ? "Female" : "Male");
            cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
            cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
            cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());

            // academic
            cmd.Parameters.AddWithValue("@HonoursYear", txtBachelorPass.Text.Trim());
            cmd.Parameters.AddWithValue("@HonoursBoard", txtBachelorBoard.Text.Trim());
            cmd.Parameters.AddWithValue("@HonoursResult", txtBachelorResult.Text.Trim());
            cmd.Parameters.AddWithValue("@HonoursGroup", txtBachelorGroup.Text.Trim());
            cmd.Parameters.AddWithValue("@MastersYear", txtMastersPass.Text.Trim());
            cmd.Parameters.AddWithValue("@MastersBoard", txtMastersBoard.Text.Trim());
            cmd.Parameters.AddWithValue("@MastersResult", txtMastersResult.Text.Trim());
            cmd.Parameters.AddWithValue("@MastersGroup", txtMastersGroup.Text.Trim());


            // Recommandation or reference
            cmd.Parameters.AddWithValue("@Organization1", txtOrgan1.Text.Trim());
            cmd.Parameters.AddWithValue("@Organization1Time", txtOrgan1Duration.Text.Trim());
            cmd.Parameters.AddWithValue("@Organization2", txtOrgan2.Text.Trim());
            cmd.Parameters.AddWithValue("@Organization2Time", txtOrgan2Duration.Text.Trim());
            cmd.Parameters.AddWithValue("@Referee1Name", txtReferee1.Text.Trim());
            cmd.Parameters.AddWithValue("@Referee1Address", txtReferee1Address.Text.Trim());
            cmd.Parameters.AddWithValue("@Referee1Email", txtReferee1Email.Text.Trim());
            cmd.Parameters.AddWithValue("@Referee2Name", txtReferee2.Text.Trim());
            cmd.Parameters.AddWithValue("@Referee2Address", txtReferee2Address.Text.Trim());
            cmd.Parameters.AddWithValue("@Referee2Email", txtReferee2Email.Text.Trim());
            cmd.Parameters.AddWithValue("@FilePath", strFilePath);
             cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Data added successfully.");
        }

        private void Reset()
        {
            CvID = 0;
            btnSave.Text = "Add";
            newFilePath = "";
            //pictureBox1.Image = null;
            using (var img = new Bitmap(Application.StartupPath + "\\images\\default_img.png"))
            {
                pictureBox1.Image = new Bitmap(img);
                lblFile.Text = "\\images\\default_img.png";
            }

            // personal info
            txtName.Text = "";
            txtFName.Text = "";
            txtMName.Text = "";
            pickDoB.Text = "";
            radioFemale.Checked = true;
            txtAddress.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";

            // career
            txtOrgan1.Text = "";
            txtOrgan1Duration.Text = "";
            txtOrgan2.Text = "";
            txtOrgan2Duration.Text = "";

            // academic
            txtBachelorBoard.Text = "";
            txtBachelorGroup.Text = "";
            txtBachelorResult.Text = "";
            txtBachelorPass.Text = "";

            txtMastersBoard.Text = "";
            txtMastersGroup.Text = "";
            txtMastersResult.Text = "";
            txtMastersPass.Text = "";


            // Recommandation or reference
            txtReferee1.Text = "";
            txtReferee1Address.Text = "";
            txtReferee1Email.Text = "";
            txtReferee2.Text = "";
            txtReferee2Address.Text = "";
            txtReferee2Email.Text = "";
        }



        private void Edit()
        {
            con.Open();
            DataTable dt = new DataTable();
            string query = @"SELECT [CvID]
                  ,[Name]
                  ,[FathersName]
                  ,[MothersName]
                  ,[DoB]
                  ,[Gender]
                  ,[Address]
                  ,[Phone]
                  ,[Email]
                  ,[HonoursYear]
                  ,[HonoursBoard]
                  ,[HonoursResult]
                  ,[HonoursGroup]

                  ,[MastersYear]
                  ,[MastersBoard]
                  ,[MastersResult]
                  ,[MastersGroup]

                  ,[Organization1]
                  ,[Organization1Time]
                  ,[Organization2]
                  ,[Organization2Time]

                  ,[Referee1Name]
                  ,[Referee1Address]
                  ,[Referee1Email]
                  ,[Referee2Name]
                  ,[Referee2Address]
                  ,[Referee2Email]
                  ,[FilePath]
              FROM [CV] WHERE [CvID] = " + CvID;
            adapt = new SqlDataAdapter(query, con);
            adapt.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                btnSave.Text = "Update";

                CvID = Convert.ToInt32(dt.Rows[0]["CvID"].ToString());
                // personal info
                txtName.Text = dt.Rows[0]["Name"].ToString();
                txtFName.Text = dt.Rows[0]["FathersName"].ToString();
                txtMName.Text = dt.Rows[0]["MothersName"].ToString();
                pickDoB.Value = Convert.ToDateTime(dt.Rows[0]["DoB"].ToString());
                radioFemale.Checked = dt.Rows[0]["Gender"].ToString() == "Female" ? true : false;
                radioMale.Checked = dt.Rows[0]["Gender"].ToString() == "Female" ? false : true;
                txtAddress.Text = dt.Rows[0]["Address"].ToString();
                txtPhone.Text = dt.Rows[0]["Phone"].ToString();
                txtEmail.Text = dt.Rows[0]["Email"].ToString();

                // career
                txtOrgan1.Text = dt.Rows[0]["Organization1"].ToString();
                txtOrgan1Duration.Text = dt.Rows[0]["Organization1Time"].ToString();
                txtOrgan2.Text = dt.Rows[0]["Organization1"].ToString();
                txtOrgan2Duration.Text = dt.Rows[0]["Organization2Time"].ToString();

                // academic
                txtBachelorBoard.Text = dt.Rows[0]["HonoursBoard"].ToString();
                txtBachelorGroup.Text = dt.Rows[0]["HonoursGroup"].ToString();
                txtBachelorResult.Text = dt.Rows[0]["HonoursResult"].ToString();
                txtBachelorPass.Text = dt.Rows[0]["HonoursYear"].ToString();

                txtMastersBoard.Text = dt.Rows[0]["MastersBoard"].ToString();
                txtMastersGroup.Text = dt.Rows[0]["MastersGroup"].ToString();
                txtMastersResult.Text = dt.Rows[0]["MastersResult"].ToString();
                txtMastersPass.Text = dt.Rows[0]["MastersYear"].ToString();

                // Recommandation or reference
                txtReferee1.Text = dt.Rows[0]["Referee1Name"].ToString();
                txtReferee1Address.Text = dt.Rows[0]["Referee1Address"].ToString();
                txtReferee1Email.Text = dt.Rows[0]["Referee1Email"].ToString();
                txtReferee2.Text = dt.Rows[0]["Referee2Name"].ToString();
                txtReferee2Address.Text = dt.Rows[0]["Referee2Address"].ToString();
                txtReferee2Email.Text = dt.Rows[0]["Referee2Email"].ToString();

                // set image
                if (dt.Rows[0]["FilePath"].ToString() != null)
                {
                    if (File.Exists(Application.StartupPath + dt.Rows[0]["FilePath"].ToString()))
                    {
                        using (var img = new Bitmap(Application.StartupPath + dt.Rows[0]["FilePath"].ToString()))
                        {
                            pictureBox1.Image = new Bitmap(img);
                            lblFile.Text = dt.Rows[0]["FilePath"].ToString();
                            isNewFile = false;
                            oldFilePath = dt.Rows[0]["FilePath"].ToString();
                        }
                    }
                    else
                    {
                        using (var img = new Bitmap(Application.StartupPath + "\\images\\default_img.png"))
                        {
                            pictureBox1.Image = new Bitmap(img);
                            lblFile.Text = "\\images\\default_img.png";
                        }
                    }
                }
                else
                {
                    using (var img = new Bitmap(Application.StartupPath + "\\images\\default_img.png"))
                    {
                        pictureBox1.Image = new Bitmap(img);
                        lblFile.Text = "\\images\\default_img.png";
                    }
                }
            }
        }


        private void Updates()
        {
            // save image
            string strFilePath = UpdateFile();

            cmd = new SqlCommand(@"UPDATE [CV]
               SET [Name] = @Name
                  ,[FathersName] = @FathersName
                  ,[MothersName] = @MothersName
                  ,[DoB] = @DoB
                  ,[Gender] = @Gender
                  ,[Address] = @Address
                  ,[Phone] = @Phone
                  ,[Email] = @Email
                  ,[HonoursYear] = @HonoursYear
                  ,[HonoursBoard] = @HonoursBoard
                  ,[HonoursResult] = @HonoursResult
                  ,[HonoursGroup] = @HonoursGroup
                  ,[MastersYear] = @MastersYear
                  ,[MastersBoard] = @MastersBoard
                  ,[MastersResult] = @MastersResult
                  ,[MastersGroup] = @MastersGroup
                  ,[Organization1] = @Organization1
                  ,[Organization1Time] = @Organization1Time
                  ,[Organization2] = @Organization2
                  ,[Organization2Time] = @Organization2Time
                  ,[Referee1Name] = @Referee1Name
                  ,[Referee1Address] = @Referee1Address
                  ,[Referee1Email] = @Referee1Email
                  ,[Referee2Name] = @Referee2Name
                  ,[Referee2Address] = @Referee2Address
                  ,[Referee2Email] = @Referee2Email
                  ,[FilePath] = @FilePath
             WHERE [CvID] = @CvID", con);
            con.Open();
            cmd.Parameters.AddWithValue("@CvID", CvID);
            // personal info
            cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@FathersName", txtFName.Text.Trim());
            cmd.Parameters.AddWithValue("@MothersName", txtMName.Text.Trim());
            cmd.Parameters.AddWithValue("@DoB", pickDoB.Value);
            cmd.Parameters.AddWithValue("@Gender", radioFemale.Checked == true ? "Female" : "Male");
            cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
            cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
            cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());

            // academic
            cmd.Parameters.AddWithValue("@HonoursYear", txtBachelorPass.Text.Trim());
            cmd.Parameters.AddWithValue("@HonoursBoard", txtBachelorBoard.Text.Trim());
            cmd.Parameters.AddWithValue("@HonoursResult", txtBachelorResult.Text.Trim());
            cmd.Parameters.AddWithValue("@HonoursGroup", txtBachelorGroup.Text.Trim());
            cmd.Parameters.AddWithValue("@MastersYear", txtMastersPass.Text.Trim());
            cmd.Parameters.AddWithValue("@MastersBoard", txtMastersBoard.Text.Trim());
            cmd.Parameters.AddWithValue("@MastersResult", txtMastersResult.Text.Trim());
            cmd.Parameters.AddWithValue("@MastersGroup", txtMastersGroup.Text.Trim());

            // Recommandation or reference
            cmd.Parameters.AddWithValue("@Organization1", txtOrgan1.Text.Trim());
            cmd.Parameters.AddWithValue("@Organization1Time", txtOrgan1Duration.Text.Trim());
            cmd.Parameters.AddWithValue("@Organization2", txtOrgan2.Text.Trim());
            cmd.Parameters.AddWithValue("@Organization2Time", txtOrgan2Duration.Text.Trim());
            cmd.Parameters.AddWithValue("@Referee1Name", txtReferee1.Text.Trim());
            cmd.Parameters.AddWithValue("@Referee1Address", txtReferee1Address.Text.Trim());
            cmd.Parameters.AddWithValue("@Referee1Email", txtReferee1Email.Text.Trim());
            cmd.Parameters.AddWithValue("@Referee2Name", txtReferee2.Text.Trim());
            cmd.Parameters.AddWithValue("@Referee2Address", txtReferee2Address.Text.Trim());
            cmd.Parameters.AddWithValue("@Referee2Email", txtReferee2Email.Text.Trim());
            cmd.Parameters.AddWithValue("@FilePath", strFilePath);
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Data updated successfully.");
        }

        private void btnBrowse_Click_1(object sender, EventArgs e)
        {
            //OpenFile();
            SelectFile();

        }


        private void OpenFile()
        {
            OpenFileDialog f = new OpenFileDialog();
            f.Filter = "JPG (*.JPG)|*.jpg";
            if (f.ShowDialog() == DialogResult.OK)
            {
                file = Image.FromFile(f.FileName);
                pictureBox1.Image = file;
                lblFile.Text = f.FileName;
            }
        }

        //string displayimg;
        string newFilePath = string.Empty;
        string oldFilePath = string.Empty;
        bool isNewFile = true;
        OpenFileDialog open = new OpenFileDialog();
        //FOLLOWING CODE FOR SELECT IMAGE AND DISPLAY IN PICTURE BOX
        private void SelectFile()
        {
            //open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp|all files|*.*";
            open.Filter = "JPG (*.JPG)|*.jpg";
            if (open.ShowDialog() == DialogResult.OK)
            {
                // display image in picture box
                //displayimg = open.SafeFileName;
                //pictureBox1.Image = new Bitmap(open.FileName);
                using (var img = new Bitmap(open.FileName))
                {
                    pictureBox1.Image = new Bitmap(img);
                }
                // image file path
                newFilePath = open.FileName;
                isNewFile = true;
            }
        }

        //FOLLOWING CODE FOR COPY SELECTED IMAGE TO THE FOLDER AND SAVE PATH TO DATABASE
        private string AddFile()
        {
            string strFilePath = string.Empty;
            if (isNewFile)
            {
                strFilePath = "\\images\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                //pictureBox1.Image.Save(Application.StartupPath + strFilePath);
                File.Copy(newFilePath, Application.StartupPath + strFilePath);
            }

            return strFilePath;
        }

        private string UpdateFile()
        {
            string strFilePath = string.Empty;
            if (isNewFile)
            {
                strFilePath = "\\images\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                //pictureBox1.Image.Save(Application.StartupPath + strFilePath);
                File.Copy(newFilePath, Application.StartupPath + strFilePath);

                //remove old file
                RemoveFile(Application.StartupPath + oldFilePath);
            }
            else
            {
                strFilePath = oldFilePath;
            }

            return strFilePath;
        }

        private void RemoveFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                if (!filePath.Contains("default"))
                {
                    File.Delete(filePath);
                }
                pictureBox1.Image = null;
            }
        }

        private void Delete()
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to remove?", "Confirm Message", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                con.Open();
                DataTable dt = new DataTable();
                string query = @"SELECT [CvID]
                      ,[Name]
                      ,[FathersName]
                      ,[MothersName]
                      ,[DoB]
                      ,[Gender]
                      ,[Address]
                      ,[Phone]
                      ,[Email]
                      ,[HonoursYear]
                      ,[HonoursBoard]
                      ,[HonoursResult]
                  ,[HonoursGroup]

                  ,[MastersYear]
                  ,[MastersBoard]
                  ,[MastersResult]
                  ,[MastersGroup]

                  ,[Organization1]
                  ,[Organization1Time]
                  ,[Organization2]
                  ,[Organization2Time]

                  ,[Referee1Name]
                  ,[Referee1Address]
                  ,[Referee1Email]
                  ,[Referee2Name]
                  ,[Referee2Address]
                  ,[Referee2Email]
                      ,[FilePath]
                  FROM [CV] WHERE [CvID] = " + CvID;
                adapt = new SqlDataAdapter(query, con);
                adapt.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["FilePath"] != null)
                    {
                        // remove old file
                        RemoveFile(Application.StartupPath + dt.Rows[0]["FilePath"].ToString());
                    }

                    string q = @"DELETE FROM [CV]
                    WHERE CvID = @CvID";
                    cmd = new SqlCommand(q, con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@CvID", CvID);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Reset();
                    LoadCVs();
                    MessageBox.Show("Data removed successfully.");

                }
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

