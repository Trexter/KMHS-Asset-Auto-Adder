using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Asset_Auto_Adder
{
    public class SQLTranslator
    {
        //System.Configuration.Configuration rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/Web");
        //System.Configuration.ConnectionStringSettings connString;
        protected String connectionString;

        public SQLTranslator()
        {

            connectionString = "Data Source=LocalHost;Initial Catalog=AssetInventory;Integrated Security=True";
            //connectionString = "Server=192.168.10.250;Database=AssetInventory; User ID=registration; Password=notthepassword;Connection Timeout=500;";
        }

        public Boolean updateAsset(string newAssetID, string serialNumber, String assetType, String assetStatus, String oldAssetID)
        {
            SqlConnection con = new SqlConnection(this.connectionString);
            con.Open();

            //update
            SqlTransaction trans = con.BeginTransaction();
            SqlCommand cmd = con.CreateCommand();

            cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "UPDATE dbo.AssetTable SET AssetNumber = @assetID, SerialNumber = @serNum, AssetType = @assetType, AssetStatus = @assetStatus WHERE AssetNumber = @firstAssetID";

            cmd.Parameters.Add("@assetID", SqlDbType.VarChar);
            cmd.Parameters.Add("@serNum", SqlDbType.VarChar);
            cmd.Parameters.Add("@assetType", SqlDbType.VarChar);
            cmd.Parameters.Add("@assetStatus", SqlDbType.VarChar);
            cmd.Parameters.Add("@firstAssetID", SqlDbType.VarChar);

            cmd.Parameters[0].Value = newAssetID;
            cmd.Parameters[1].Value = serialNumber;
            cmd.Parameters[2].Value = assetType;
            cmd.Parameters[3].Value = assetStatus;
            cmd.Parameters[4].Value = oldAssetID;

            cmd.ExecuteNonQuery();
            trans.Commit();
            con.Close();
    
            return true;
        }

        public int reportIncident(string assetID, string userID, string incidentType, string incidentDescription)
        {
            
            SqlConnection con = new SqlConnection(this.connectionString);
            con.Open();

            //reports the incident
            SqlTransaction trans = con.BeginTransaction();
            SqlCommand cmd = con.CreateCommand();

            cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO dbo.AssetIncidentTable (AssetID, UserID, IncidentType, IncidentDescription, IncidentDate) VALUES (@assetID, @userID, @incidentType, @incidentDesc, @incidentDate);";

            cmd.Parameters.Add("@assetID", SqlDbType.VarChar);
            cmd.Parameters.Add("@userID", SqlDbType.VarChar);
            cmd.Parameters.Add("@incidentType", SqlDbType.VarChar);
            cmd.Parameters.Add("@incidentDesc", SqlDbType.VarChar);
            cmd.Parameters.Add("@incidentDate", SqlDbType.DateTime);

            cmd.Parameters[0].Value = assetID;
            cmd.Parameters[1].Value = userID;
            cmd.Parameters[2].Value = incidentType;
            cmd.Parameters[3].Value = incidentDescription;
            cmd.Parameters[4].Value = DateTime.Now;

            //execute command and commit it 
            if (cmd.ExecuteNonQuery() == 1)
            {
                
                trans.Commit();
                con.Close();
                //reported incident without issues
                return 1;
            }
            else
            {
                trans.Commit();
                con.Close();
                //could not report incident
                return 2;
            }
            
            
        }

        public int addAssetToSQLDatabase(String assetType, String serialNumber, String assetNumber, String userID)
        {
            
            //checks if the asset has already been added
            if (this.checkAsset(assetNumber))
            {
                return 3;
            }
            else
            {

                SqlConnection con = new SqlConnection(this.connectionString);
                con.Open();

                //inserts the data into the correct data base
                SqlTransaction trans = con.BeginTransaction();
                SqlCommand cmd = con.CreateCommand();

                cmd.Transaction = trans;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO dbo.AssetTable (AssetNumber, SerialNumber, AssetType, AssetStatus, DateAdded) VALUES (@assetNum, @serNum, @assetType, '0', @dateAdded);";

                cmd.Parameters.Add("@assetNum", SqlDbType.VarChar);
                cmd.Parameters.Add("@serNum", SqlDbType.VarChar);
                cmd.Parameters.Add("@assetType", SqlDbType.VarChar);
                cmd.Parameters.Add("@dateAdded", SqlDbType.DateTime);

                cmd.Parameters[0].Value = assetNumber;
                cmd.Parameters[1].Value = serialNumber;
                cmd.Parameters[2].Value = assetType;
                cmd.Parameters[3].Value = DateTime.Now;


                if (cmd.ExecuteNonQuery() == 1)
                {
                    //commit the table addition
                    trans.Commit();
                    con.Close();
                    //ckecks to see if a userid was entered
                    if (userID == null || userID == "")
                    {
                        //asset was added and left unassigned
                        return 1;
                    }
                    else
                    {
                        //try to link the asset to the userid
                        int err1 = this.linkAsset(assetNumber, userID);
                        this.modifyAssetStatus(assetNumber, "1");
                        return err1 + 3;
                        
                    }
                }
                else
                {
                    //command could not be executed
                    return 2;
                }
            }


        }

        public string getUserIDFromAssetID(string assetID)
        {
            SqlConnection con = new SqlConnection(this.connectionString);
            con.Open();

            //checks if the asset already exists
            SqlTransaction trans = con.BeginTransaction();
            SqlCommand cmd = con.CreateCommand();

            cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM dbo.AssetTrackingTable WHERE AssetNumber = @assetNum AND DateRetracted is null";

            cmd.Parameters.Add("@assetNum", SqlDbType.VarChar);

            cmd.Parameters[0].Value = assetID;

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string temp = reader["UserID"].ToString();
                reader.Close();
                con.Close();
                
                return temp;
            }
            else
            {
                reader.Close();
                con.Close();
                return "NOT ASSIGNED!";
                
            }
        }

        public Boolean modifyAssetStatus(string assetNumber, string assetStatus)
        {
            SqlConnection con = new SqlConnection(this.connectionString);
            con.Open();

            //modifies the status of the asset
            SqlTransaction trans = con.BeginTransaction();
            SqlCommand cmd = con.CreateCommand();

            cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "UPDATE dbo.AssetTable SET AssetStatus = @assetStat WHERE AssetNumber = @assetNum";

            cmd.Parameters.Add("@assetStat", SqlDbType.VarChar);
            cmd.Parameters.Add("@assetNum", SqlDbType.VarChar);

            cmd.Parameters[0].Value = assetStatus;
            cmd.Parameters[1].Value = assetNumber;

            //execute command and commit it 
            if (cmd.ExecuteNonQuery() == 1)
            {
                trans.Commit();
                con.Close();
                return true;
            }
            else
            {
                trans.Commit();
                con.Close();
                return false;
            }
        }

        public int linkAsset(string assetNumber, string userID)
        {
            SqlConnection con = new SqlConnection(this.connectionString);
            con.Open();

            SqlTransaction trans;
            SqlCommand cmd;

            //check to see if asset has been added...
            if(this.checkAsset(assetNumber))
            {
                //check if the asset has been assigned to another userID
                if(this.checkForPreviousAssignmentAndRetract(assetNumber))
                {
                    //tries to add a new row
                    trans = con.BeginTransaction();
                    cmd = con.CreateCommand();

                    cmd.Transaction = trans;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO dbo.AssetTrackingTable (AssetNumber, UserID, DateAssigned) VALUES (@assetNum, @userID, @dateAssign);";

                    cmd.Parameters.Add("@assetNum", SqlDbType.VarChar);
                    cmd.Parameters.Add("@dateAssign", SqlDbType.DateTime);
                    cmd.Parameters.Add("@userID", SqlDbType.VarChar);

                    cmd.Parameters[0].Value = assetNumber;
                    cmd.Parameters[1].Value = DateTime.Now;
                    cmd.Parameters[2].Value = userID;

                    //execute command and commit it 
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        trans.Commit();
                        con.Close();
                        //this.modifyAssetStatus(assetNumber, "1");
                        return 2;
                    }
                    else
                    {
                        trans.Commit();
                        con.Close();
                        return 3;
                    }
                }
                //if it hasnt
                else 
                {
                    //tries to add a new row
                    trans = con.BeginTransaction();
                    cmd = con.CreateCommand();

                    cmd.Transaction = trans;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO dbo.AssetTrackingTable (AssetNumber, UserID, DateAssigned) VALUES (@assetNum, @userID, @dateAssign);";

                    cmd.Parameters.Add("@assetNum", SqlDbType.VarChar);
                    cmd.Parameters.Add("@dateAssign", SqlDbType.DateTime);
                    cmd.Parameters.Add("@userID", SqlDbType.VarChar);

                    cmd.Parameters[0].Value = assetNumber;
                    cmd.Parameters[1].Value = DateTime.Now;
                    cmd.Parameters[2].Value = userID;

                    //execute command and commit it 
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        trans.Commit();
                        con.Close();
                        return 2;
                    }
                    else
                    {
                        trans.Commit();
                        con.Close();
                        return 5;
                    }
                }
            }
            else
            {
                //asset has not been added
                return 1;
            }
        }

        public Boolean checkForPreviousAssignmentAndRetract(string assetNumber)
        {
            SqlConnection con = new SqlConnection(this.connectionString);
            con.Open();

            //checks if the asset has a unretracted row
            SqlTransaction trans = con.BeginTransaction();
            SqlCommand cmd = con.CreateCommand();

            cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "UPDATE dbo.AssetTrackingTable SET DateRetracted = @dateRetract WHERE AssetNumber = @assetNum AND DateRetracted is null";

            cmd.Parameters.Add("@assetNum", SqlDbType.VarChar);
            cmd.Parameters.Add("@dateRetract", SqlDbType.DateTime);

            cmd.Parameters[0].Value = assetNumber;
            cmd.Parameters[1].Value = DateTime.Now;

            //execute command and commit it 
            if(cmd.ExecuteNonQuery() == 1)
            {
                trans.Commit();
                con.Close();
                return true;
            }
            else
            {
                trans.Commit();
                con.Close();
                return false;
            }
        }

        public Boolean checkAsset(String assetNumber)
        {
            SqlConnection con = new SqlConnection(this.connectionString);
            con.Open();

            //checks if the asset already exists
            SqlTransaction trans = con.BeginTransaction();
            SqlCommand cmd = con.CreateCommand();

            cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM dbo.AssetTable WHERE AssetNumber = @assetNum";

            cmd.Parameters.Add("@assetNum", SqlDbType.VarChar);

            cmd.Parameters[0].Value = assetNumber;

            SqlDataReader reader = cmd.ExecuteReader();

            if(reader.Read())
            {
                reader.Close();
                con.Close();
                return true;
            }
            else
            {
                reader.Close();
                con.Close();
                return false;
            }
        }

        public Boolean checkCredentials(string UID, string PW)
        {
            SqlConnection con = new SqlConnection(this.connectionString);
            con.Open();

            //the command
            SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Users WHERE UserID = @uid AND Password = @pw", con);

            //setup and add parameters
            cmd.Parameters.Add("@uid", SqlDbType.NChar);
            cmd.Parameters.Add("@pw", SqlDbType.NChar);

            cmd.Parameters[0].Value = UID;
            cmd.Parameters[1].Value = PW;

            //execute command and setup reader
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                reader.Close();
                con.Close();
                return true;
            }
            else
            {
                reader.Close();
                con.Close();
                return false;
            }

        }

        public DataSet DataSourceCreator(string cmdinput)
        {
            SqlConnection con = new SqlConnection(this.connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand(cmdinput, con);

            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter();

            da.SelectCommand = cmd;

            DataSet ds = new DataSet();
            da.Fill(ds, "AssetNumber");

            con.Close();
            return ds;
        }

    }
}