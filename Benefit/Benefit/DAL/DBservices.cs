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

        sb.AppendFormat("Values({0},{1},{2},{3},{4})", UserCode.ToString(), t.PersonalTrainingPrice.ToString(), t.GroupTrainingPrice.ToString());
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
