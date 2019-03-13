using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Text;
using Benefit.Models;

/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservices
{
    public SqlDataAdapter da;
    public DataTable dt;

    public DBservices()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {


        string pStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
        SqlConnection con = new SqlConnection(pStr);
        con.Open();
        return con;
    }

    //--------------------------------------------------------------------------------------------------
    // This method inserts a car to the cars table 
    //--------------------------------------------------------------------------------------------------

    public int SignInTrainee(Trainee t)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        String pStr1 = BuildInsertUserCommand(t);  
        cmd = CreateCommand(pStr1, con);           
        
        try
        {
            int UserCode = Convert.ToInt32(cmd.ExecuteScalar());
            InsertSportCategories(t.SportCategories, UserCode);
            String pStr2 = BuildInsertTraineeCommand(UserCode, t);
            cmd = CreateCommand(pStr2, con);
            cmd.ExecuteNonQuery();
            return UserCode;
        }
        catch (Exception ex)
        {
            return 0;
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public int SignInTrainer(Trainer t)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        String pStr = BuildInsertUserCommand(t);
        cmd = CreateCommand(pStr, con);

        try
        {
            int UserCode = Convert.ToInt32(cmd.ExecuteScalar());
            InsertSportCategories(t.SportCategories, UserCode);
            pStr = BuildInsertTrainerCommand(UserCode, t);
            cmd = CreateCommand(pStr, con);
            cmd.ExecuteNonQuery();
            return UserCode;
        }
        catch (Exception ex)
        {
            return 0;
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public bool InsertSportCategories(int[] SportCategories, int UserCode)
    {
        SqlConnection con;
        SqlCommand cmd;
        try
        {
            con = connect("BenefitConnectionStringName");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            for (int i = 0; i < SportCategories.Length; i++)
            {
                String str = BuildInsertSportCategoriesCommand(UserCode, SportCategories[i] );
                cmd = CreateCommand(str, con);
                int numEffected = cmd.ExecuteNonQuery();
                
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
            throw (ex);
        }

    }

    public bool CheckIfEmailExists(string UserEmail)
    {

        SqlConnection con = null;

        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "select * from Users where Users.Email='" + UserEmail + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); 

            if (dr.Read())
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }

    }

    public Trainee CheckIfPasswordMatches(string UserEmail, string Password)
    {

        SqlConnection con = null;

        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "select Users.UserCode, Users.IsTrainer from Users where Users.Email='"+UserEmail+"' and Users.Password= '" + Password + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            Trainee t = new Trainee();
            if (dr.Read())
            {
                
                t.UserCode = Convert.ToInt32(dr["UserCode"]);
                t.IsTrainer = Convert.ToInt32(dr["IsTrainer"]);
                return t;
            }
            else
            {
                t.UserCode = 0;
                return t;
            }

        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }

    }

    public List<Trainee> SearchPartners(OnlineHistoryTrainee o)
    {

        SqlConnection con= null;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");
            String selectSTR = "select U.Gender, U.SearchRadius, datediff(year, U.DateOfBirth, getdate()) as Age, T.MinBudget, T.MaxBudget, T.TrainerGender,T.PartnerGender, T.MinPartnerAge, T.MaxPartnerAge, USC.CategoryCode from Users as U inner join Trainees as T on U.UserCode = T.TraineeCode inner join UserSportCategories as USC on U.UserCode = USC.UserCode where U.UserCode = '" + o.UserCode + "'";
            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
           
            List<int> scl = new List<int>();
            string gender = null;
            int searchRadius =0;
            int age = 0;
            string partnerGender= null ;
            int minPartnerAge = 0;
            int maxPartnerAge =0;
            
            while (dr.Read())
            {
             gender = Convert.ToString(dr["Gender"]);
             searchRadius = Convert.ToInt32(dr["SearchRadius"]);
             age = Convert.ToInt32(dr["Age"]);
             partnerGender = Convert.ToString(dr["PartnerGender"]);
             minPartnerAge = Convert.ToInt32(dr["MinPartnerAge"]);
             maxPartnerAge = Convert.ToInt32(dr["MaxPartnerAge"]);
             int sc = Convert.ToInt32(dr["CategoryCode"]);
                scl.Add(sc);
            }

            selectSTR = "select U.FirstName, U.LastName, U.SearchRadius, U.Gender, datediff(year,  U.DateOfBirth, getdate()) as Age, OHT.Longitude, OHT.Latitude, OHT.StartTime, OHT.EndTime, USC.CategoryCode " +
               "from Users as U inner join Trainees as T on U.UserCode = T.TraineeCode " +
               "inner join UserSportCategories as USC on U.UserCode = USC.UserCode " +
               "inner join OnlineHistoryTrainee as OHT on OHT.TraineeCode = U.UserCode " +

               "where OHT.WithPartner = 1 " +
               "and U.UserCode <> " + o.UserCode + " " +
               "and OHT.StartTime <= '" + o.EndTime + "' and OHT.EndTime >= '" + o.StartTime + "' " +
               "and U.gender = '" + gender + "' " +
               "and T.PartnerGender = '" + partnerGender + "' " +
               "and datediff(year, U.DateOfBirth, getdate()) between " + minPartnerAge + " and " + maxPartnerAge;
            cmd = new SqlCommand(selectSTR, con);
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<Trainee> t = new List<Trainee>();
            Trainee tr = new Trainee();
            tr.FirstName = "success!!!!!";
            t.Add(tr);
            return t;


        }
        catch (Exception ex)
        {
            throw (ex);
        }



        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public List<Trainee> InsertOnlineTrainee(OnlineHistoryTrainee o)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        String pStr = BuildInsertOnlineHistoryTraineeCommand(o);
        cmd = CreateCommand(pStr, con);

        try
        {
            int OnlineCode = Convert.ToInt32(cmd.ExecuteScalar());
            String pStr2= BuildInsertCurrentTraineeCommand(OnlineCode);
            cmd = CreateCommand(pStr2, con);
            cmd.ExecuteNonQuery();
            return SearchPartners(o);
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public void InsertOnlineTrainer(OnlineHistoryTrainer o)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        String pStr = BuildInsertOnlineHistoryTrainerCommand(o);
        cmd = CreateCommand(pStr, con);

        try
        {
            int OnlineCode = Convert.ToInt32(cmd.ExecuteScalar());
            String pStr2 = BuildInsertCurrentTrainerCommand(OnlineCode);
            cmd = CreateCommand(pStr2, con);
            cmd.ExecuteNonQuery();
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------
    // Build the Insert command String
    //--------------------------------------------------------------------
    private String BuildInsertUserCommand(User u)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},{8},{9})", u.Email, u.FirstName, u.LastName, u.Password, u.Gender, u.DateOfBirth, u.Picture, u.SearchRadius.ToString(), u.IsTrainer.ToString(), u.Rate.ToString());
        String prefix = "INSERT INTO Users (Email, FirstName, LastName, Password, Gender, DateOfBirth, Picture, SearchRadius, IsTrainer, Rate) output INSERTED.UserCode ";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertTraineeCommand(int UserCode, Trainee t)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values({0},{1},{2},'{3}','{4}')", UserCode.ToString(), t.MinBudget.ToString(), t.MaxBudget.ToString(), t.PartnerGender, t.TrainerGender);
        String prefix = "INSERT INTO Trainees (TraineeCode, MinBudget, MaxBudget, PartnerGender, TrainerGender) ";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertTrainerCommand(int UserCode, Trainer t)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values({0},{1},{2})", UserCode.ToString(), t.PersonalTrainingPrice.ToString(), t.GroupTrainingPrice.ToString());
        String prefix = "INSERT INTO Trainers (TrainerCode, PersonalTrainingPrice, GroupTrainingPrice) ";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertSportCategoriesCommand(int UserCode, int CategoryCode)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values({0},{1})", UserCode.ToString(), CategoryCode.ToString());
        String prefix = "INSERT INTO UserSportCategories (UserCode, CategoryCode) ";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertOnlineHistoryTraineeCommand(OnlineHistoryTrainee o)
    {
        String command;
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("Values({0},'{1}','{2}','{3}','{4}', '{5}', {6}, {7}, {8}, {9} )", o.UserCode.ToString(), o.InsertTime, o.Latitude, o.Longitude, o.StartTime, o.EndTime, o.WithTrainer.ToString(), o.WithPartner.ToString(), o.GroupWithTrainer.ToString(), o.GroupWithPartners.ToString());
        String prefix = "INSERT INTO OnlineHistoryTrainee (TraineeCode, InsertTime, Latitude, Longitude, StartTime, EndTime, WithTrainer,WithPartner, GroupWithTrainer, GroupWithPartners) output INSERTED.OnlineCode  ";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertCurrentTraineeCommand(int OnlineCode)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values({0})", OnlineCode.ToString());
        String prefix = "INSERT INTO CurrentOnlineTrainee (OnlineCode)";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertOnlineHistoryTrainerCommand(OnlineHistoryTrainer o)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values({0},'{1}','{2}','{3}','{4}', '{5}')", o.UserCode.ToString(), o.InsertTime, o.Latitude, o.Longitude, o.StartTime, o.EndTime);
        String prefix = "INSERT INTO OnlineHistoryTrainer (TrainerCode, InsertTime, Latitude, Longitude, StartTime, EndTime) output INSERTED.OnlineCode  ";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertCurrentTrainerCommand(int OnlineCode)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values({0})", OnlineCode.ToString());
        String prefix = "INSERT INTO CurrentOnlineTrainer (OnlineCode)";
        command = prefix + sb.ToString();
        return command;
    }



    //---------------------------------------------------------------------------------
    // Create the SqlCommand
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = CommandSTR;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

        return cmd;
    }



}
