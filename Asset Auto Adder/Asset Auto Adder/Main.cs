using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using System.IO;
using System.Reflection;

namespace Asset_Auto_Adder
{
    public partial class Main : Form
    {
        
        //declare sound player
        SoundPlayer VictoryPlayer;
        //sql stuff
        SQLTranslator sqlTran = new SQLTranslator();

        public Main()
        {
            InitializeComponent();
            addAssets.Enabled = false;
            Assembly _assembly = Assembly.GetExecutingAssembly();
            Stream _soundStream = _assembly.GetManifestResourceStream("Asset_Auto_Adder.Media.tada.wav");
            
            VictoryPlayer = new SoundPlayer(_soundStream);
            VictoryPlayer.Play();

            assetTypetextBox.Text = "Ipad Air 16GB";
        }

        private void chooseFile_Click(object sender, EventArgs e)
        {
            
            openFileDialog.ShowDialog();
        }

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            //Console.Beep();
            filePath.Text = openFileDialog.FileName;
            addAssets.Enabled = true;
            DataSet ds = new DataSet();
            ds.Tables.Add("Assets");
            ds.Tables["Assets"].Columns.Add("User ID");
            ds.Tables["Assets"].Columns.Add("Asset ID");
            ds.Tables["Assets"].Columns.Add("Serial Number");

            string[] rows = File.ReadAllText(openFileDialog.FileName).Split("\r".ToCharArray());

            for(int i = 0; i < rows.Length; i++)
            {
                string[] items = rows[i].Split(',');
                string[] items2 = new string[3];
                items2[0] = items[0];
                items2[1] = items[1];
                items2[2] = items[2];

                ds.Tables["Assets"].Rows.Add(items2);
            }
            CSVGrid.DataSource = ds.Tables[0].DefaultView;
        }

        private void addAssets_Click(object sender, EventArgs e)
        {
            
            //start the new thread
            Thread sqlThread = new Thread(new ThreadStart(sqlWriterThreadStart));
            sqlThread.Start();
            
        }

        private void progressBar_Click(object sender, EventArgs e)
        {

        }

        //thread for sql stuff---------------------------------------thread

        public void sqlWriterThreadStart()
        {
            sqlAddAssets();
        }

        public void sqlAddAssets()
        {
            //sets up the data set for the csv file

            StreamReader sr = new StreamReader(openFileDialog.FileName);
            String csvData = File.ReadAllText(openFileDialog.FileName);
            String[] csvRows = csvData.Split("\r".ToCharArray());
            //set the max val
            progressBar.BeginInvoke(
                    new Action(() =>
                    {
                        progressBar.Maximum = csvRows.Length;
                    }
            ));

            //get the asset type selected
            String assetType = "none chosen";
            assetTypetextBox.BeginInvoke(
                new Action(() =>
                {
                    assetType = assetTypetextBox.Text.ToString();
                }
            ));

            //adds the assets to the sql database
            for(int i = 0; i < csvRows.Length; i++)
            {
                String[] columnVals = csvRows[i].Split(',');

                int USERID = 0;
                int ASSETID = 1;
                int SERIALNUMBER = 2;

                //adding assets to database
                

                int errorCode = sqlTran.addAssetToSQLDatabase(assetType, columnVals[SERIALNUMBER], columnVals[ASSETID], columnVals[USERID]);

                switch(errorCode)
                {
                    case 3:
                        DetailsBox.BeginInvoke(
                            new Action(() =>
                            {
                                DetailsBox.SelectionColor = System.Drawing.Color.Red;
                                DetailsBox.AppendText("ERROR! Asset was already added to the database! nothing was done with this asset" + Environment.NewLine);
                                DetailsBox.ScrollToCaret();
                                DetailsBox.SelectionColor = System.Drawing.Color.White;
                            }
                            ));
                        break;
                    case 1:
                        DetailsBox.BeginInvoke(
                            new Action(() =>
                            {
                                DetailsBox.SelectionColor = System.Drawing.Color.Yellow;
                                DetailsBox.AppendText("Added Asset ID: " + columnVals[ASSETID] + " Serial Number: " + columnVals[SERIALNUMBER] + "However, the asset was left unassigned because there was no User ID specified" + Environment.NewLine);
                                DetailsBox.ScrollToCaret();
                                DetailsBox.SelectionColor = System.Drawing.Color.White;
                            }
                            ));
                        break;
                    case 2:
                        DetailsBox.BeginInvoke(
                            new Action(() =>
                            {
                                DetailsBox.SelectionColor = System.Drawing.Color.Red;
                                DetailsBox.AppendText("ERROR! Asset could not be added!" + Environment.NewLine);
                                DetailsBox.ScrollToCaret();
                                DetailsBox.SelectionColor = System.Drawing.Color.White;
                            }
                            ));
                        break;
                    case 5:
                        DetailsBox.BeginInvoke(
                            new Action(() =>
                            {
                                DetailsBox.SelectionColor = System.Drawing.Color.Green;
                                DetailsBox.AppendText("Added Asset ID: " + columnVals[ASSETID] + " Serial Number: " + columnVals[SERIALNUMBER] + "AND Asset was linked!" + Environment.NewLine);
                                DetailsBox.ScrollToCaret();
                                DetailsBox.SelectionColor = System.Drawing.Color.White;
                            }
                            ));
                        break;
                    case 4:
                        DetailsBox.BeginInvoke(
                            new Action(() =>
                            {
                                DetailsBox.SelectionColor = System.Drawing.Color.Yellow;
                                DetailsBox.AppendText("Added Asset ID: " + columnVals[ASSETID] + " Serial Number: " + columnVals[SERIALNUMBER] + "However, ASSET COULD NOT BE LINKED TO SPECIFIED USER!!" + Environment.NewLine);
                                DetailsBox.ScrollToCaret();
                                DetailsBox.SelectionColor = System.Drawing.Color.White;
                            }
                            ));
                        break;
                    default:
                        DetailsBox.BeginInvoke(
                            new Action(() =>
                            {
                                DetailsBox.SelectionColor = System.Drawing.Color.Red;
                                DetailsBox.AppendText("UNKNOWN ERROR CHECK SOURCE CODE!" + Environment.NewLine);
                                DetailsBox.ScrollToCaret();
                                DetailsBox.SelectionColor = System.Drawing.Color.White;
                            }
                            ));
                        break;

                }



                progressBar.BeginInvoke(
                    new Action(() =>
                    {
                        progressBar.Value = i;
                    }
                ));

                Thread.Sleep(10);
            }
            VictoryPlayer.Play();
        }
    }

}
