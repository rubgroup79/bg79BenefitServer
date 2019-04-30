using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Text;
using Benefit.Models;
using System.Dynamic;
using Newtonsoft.Json.Linq;

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
                String str = BuildInsertSportCategoriesCommand(UserCode, SportCategories[i]);
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

            String selectSTR = "select Users.UserCode, Users.IsTrainer from Users where Users.Email='" + UserEmail + "' and Users.Password= '" + Password + "'";
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

    public List<Result> SearchPartners(OnlineHistoryTrainee o)
    {

        SqlConnection con = null;
        SqlConnection con1 = null;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");

            //Get trainee's details that needed for the search
            String selectSTR = "select U.Gender, U.SearchRadius, datediff(year, U.DateOfBirth, getdate()) as Age, T.PartnerGender, T.MinPartnerAge, T.MaxPartnerAge, USC.CategoryCode from Users as U inner join Trainees as T on U.UserCode = T.TraineeCode inner join UserSportCategories as USC on U.UserCode = USC.UserCode where U.UserCode = '" + o.UserCode + "'";
            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<int> scl = new List<int>();
            string search_Gender = null;
            int search_SearchRadius = 0;
            int search_Age = 0;
            string search_PartnerGender = null;
            int search_MinPartnerAge = 0;
            int search_MaxPartnerAge = 0;

            while (dr.Read())
            {
                search_Gender = Convert.ToString(dr["Gender"]);
                search_SearchRadius = Convert.ToInt32(dr["SearchRadius"]);
                search_Age = Convert.ToInt32(dr["Age"]);
                search_PartnerGender = Convert.ToString(dr["PartnerGender"]);
                search_MinPartnerAge = Convert.ToInt32(dr["MinPartnerAge"]);
                search_MaxPartnerAge = Convert.ToInt32(dr["MaxPartnerAge"]);
                int sc = Convert.ToInt32(dr["CategoryCode"]);
                scl.Add(sc);
            }

            //if user doesnt care of the partner's gender
            string partnerGenderStr = null;
            if (search_PartnerGender == "Both")
                partnerGenderStr = " ";
            else partnerGenderStr = "and (U.gender = '" + search_PartnerGender + "') ";

            string sportCategoriesStr = "and (USC.CategoryCode = " + scl[0];
            for (int i = 1; i < scl.Count; i++)
            {
                sportCategoriesStr += " or USC.CategoryCode = " + scl[i];
            }
            sportCategoriesStr += ") ";


            con1 = connect("BenefitConnectionStringName");
            selectSTR = "select distinct U.UserCode, U.FirstName, U.LastName, datediff(year,  U.DateOfBirth, getdate()) as Age, U.Gender, OHT.Latitude, OHT.Longitude, OHT.StartTime, OHT.EndTime, T.PartnerGender, U.SearchRadius, U.Picture, U.IsTrainer  " +
               "from Users as U inner join Trainees as T on U.UserCode = T.TraineeCode " +
               "inner join OnlineHistoryTrainee as OHT on OHT.TraineeCode = U.UserCode " +
               "inner join CurrentOnlineTrainee as COT on COT.OnlineCode = OHT.OnlineCode " +
               "inner join UserSportCategories as USC on USC.UserCode= U.UserCode " +
               "where OHT.WithPartner = 1 " +
               "and (U.UserCode <> " + o.UserCode + ") " +
               "and (datediff(year, U.DateOfBirth, getdate()) between " + search_MinPartnerAge + " and " + search_MaxPartnerAge + ") " +
               "and (" + search_Age + ">= T.MinPartnerAge and " + search_Age + "<= T.MaxPartnerAge) " +
               partnerGenderStr + " " +
               "and (OHT.StartTime <= '" + o.EndTime + "' and OHT.EndTime >= '" + o.StartTime + "') " +
               sportCategoriesStr;

            SqlCommand cmd1 = new SqlCommand(selectSTR, con1);
            SqlDataReader dr1 = cmd1.ExecuteReader(CommandBehavior.CloseConnection);
            List<Result> tl = new List<Result>();

            while (dr1.Read())
            {
                string result_PartnerGender = Convert.ToString(dr1["PartnerGender"]);
                double result_Longitude = Convert.ToDouble(dr1["Longitude"]);
                double result_Latitude = Convert.ToDouble(dr1["Latitude"]);
                double distance = distances(result_Latitude, result_Longitude, Convert.ToDouble(o.Latitude), Convert.ToDouble(o.Longitude), 'K');
                int SearchRaduis = Convert.ToInt32(dr1["SearchRadius"]);
                if ((distance <= SearchRaduis + search_SearchRadius) && (result_PartnerGender == "Both" || result_PartnerGender == search_Gender))
                {
                    Result rt = new Result();
                    rt.UserCode = Convert.ToInt32(dr1["UserCode"]);
                    rt.FirstName = Convert.ToString(dr1["FirstName"]);
                    rt.LastName = Convert.ToString(dr1["LastName"]);
                    rt.Age = Convert.ToInt32(dr1["Age"]);
                    rt.Gender = Convert.ToString(dr1["Gender"]);
                    rt.Longitude = Convert.ToSingle(result_Longitude);
                    rt.Latitude = Convert.ToSingle(result_Latitude);
                    rt.StartTime = Convert.ToString(dr1["StartTime"]);
                    rt.EndTime = Convert.ToString(dr1["EndTime"]);
                    rt.Picture = Convert.ToString(dr1["Picture"]);
                    rt.IsTrainer = Convert.ToInt32(dr1["IsTrainer"]);
                    rt.Distance = distance;
                    tl.Add(rt);

                }

            }
            return tl;

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

    public List<Result> SearchTrainers(OnlineHistoryTrainee o)
    {

        SqlConnection con = null;
        SqlConnection con1 = null;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");

            //Get trainee's details that needed for the search
            String selectSTR = "select U.Gender, U.SearchRadius, datediff(year, U.DateOfBirth, getdate()) as Age, T.MinBudget, T.MaxBudget, T.TrainerGender, USC.CategoryCode from Users as U inner join Trainees as T on U.UserCode = T.TraineeCode inner join UserSportCategories as USC on U.UserCode = USC.UserCode where U.UserCode = '" + o.UserCode + "'";
            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<int> scl = new List<int>();
            string search_Gender = null;
            int search_SearchRadius = 0;
            int search_Age = 0;
            int search_MinBudget = 0;
            int search_MaxBudget = 0;
            string search_TrainerGender = null;

            while (dr.Read())
            {
                search_Gender = Convert.ToString(dr["Gender"]);
                search_SearchRadius = Convert.ToInt32(dr["SearchRadius"]);
                search_Age = Convert.ToInt32(dr["Age"]);
                search_MinBudget = Convert.ToInt32(dr["MinBudget"]);
                search_MaxBudget = Convert.ToInt32(dr["MaxBudget"]);
                search_TrainerGender = Convert.ToString(dr["TrainerGender"]);
                int sc = Convert.ToInt32(dr["CategoryCode"]);
                scl.Add(sc);
            }

            //if user doesnt care of the partner's gender
            string trainerGenderStr = null;
            if (search_TrainerGender == "Both")
                trainerGenderStr = " ";
            else trainerGenderStr = " (U.gender = '" + search_TrainerGender + " and ') ";

            string sportCategoriesStr = "and (USC.CategoryCode = " + scl[0];
            for (int i = 1; i < scl.Count; i++)
            {
                sportCategoriesStr += " or USC.CategoryCode = " + scl[i];
            }
            sportCategoriesStr += ") ";


            con1 = connect("BenefitConnectionStringName");
            selectSTR = "select distinct U.UserCode, U.FirstName, U.LastName, datediff(year, U.DateOfBirth, getdate()) as Age, U.Gender, OHT.Latitude, OHT.Longitude, OHT.StartTime, OHT.EndTime, U.SearchRadius, T.PersonalTrainingPrice, U.Picture, U.IsTrainer " +
                "from Users as U inner join Trainers as T on U.UserCode = T.TrainerCode " +
                "inner join OnlineHistoryTrainer as OHT on OHT.TrainerCode = U.UserCode " +
                "inner join CurrentOnlineTrainer as COT on COT.OnlineCode = OHT.OnlineCode " +
                "inner join UserSportCategories as USC on USC.UserCode = U.UserCode " +
                "where " + trainerGenderStr +
                "(T.PersonalTrainingPrice between " + search_MinBudget + " and " + search_MaxBudget + ") " +
                "and(OHT.StartTime <= '" + o.EndTime + "' and OHT.EndTime >= '" + o.StartTime + "') " +
                sportCategoriesStr;

            SqlCommand cmd1 = new SqlCommand(selectSTR, con1);
            SqlDataReader dr1 = cmd1.ExecuteReader(CommandBehavior.CloseConnection);
            List<Result> tl = new List<Result>();

            while (dr1.Read())
            {
                double result_Longitude = Convert.ToDouble(dr1["Longitude"]);
                double result_Latitude = Convert.ToDouble(dr1["Latitude"]);
                double distance = distances(result_Latitude, result_Longitude, Convert.ToDouble(o.Latitude), Convert.ToDouble(o.Longitude), 'K');
                int SearchRaduis = Convert.ToInt32(dr1["SearchRadius"]);
                if ((distance <= SearchRaduis + search_SearchRadius))
                {
                    Result rt = new Result();
                    rt.UserCode = Convert.ToInt32(dr1["UserCode"]);
                    rt.FirstName = Convert.ToString(dr1["FirstName"]);
                    rt.LastName = Convert.ToString(dr1["LastName"]);
                    rt.Age = Convert.ToInt32(dr1["Age"]);
                    rt.Gender = Convert.ToString(dr1["Gender"]);
                    rt.Longitude = Convert.ToSingle(result_Longitude);
                    rt.Latitude = Convert.ToSingle(result_Latitude);
                    rt.StartTime = Convert.ToString(dr1["StartTime"]);
                    rt.EndTime = Convert.ToString(dr1["EndTime"]);
                    rt.Picture = Convert.ToString(dr1["Picture"]);
                    rt.Price = Convert.ToInt32(dr1["PersonalTrainingPrice"]);
                    rt.IsTrainer = Convert.ToInt32(dr1["IsTrainer"]);
                    rt.Distance = distance;
                    tl.Add(rt);

                }

            }
            return tl;

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

    public List<HistoryGroupTraining> SearchGroups(OnlineHistoryTrainee o)
    {

        SqlConnection con = null;
        SqlConnection con1 = null;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");

            //Get trainee's details that needed for the search
            String selectSTR = "select U.SearchRadius, T.MinBudget, T.MaxBudget, USC.CategoryCode from Users as U inner join Trainees as T on U.UserCode = T.TraineeCode inner join UserSportCategories as USC on U.UserCode = USC.UserCode where U.UserCode = '" + o.UserCode + "'";
            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<int> scl = new List<int>();
            int search_SearchRadius = 0;
            int search_MinBudget = 0;
            int search_MaxBudget = 0;

            while (dr.Read())
            {
                search_SearchRadius = Convert.ToInt32(dr["SearchRadius"]);
                search_MinBudget = Convert.ToInt32(dr["MinBudget"]);
                search_MaxBudget = Convert.ToInt32(dr["MaxBudget"]);
                int sc = Convert.ToInt32(dr["CategoryCode"]);
                scl.Add(sc);
            }

            string sportCategoriesStr = "and (HGT.SportCategoryCode = " + scl[0];
            for (int i = 1; i < scl.Count; i++)
            {
                sportCategoriesStr += " or HGT.SportCategoryCode = " + scl[i];
            }
            sportCategoriesStr += ") ";

            string GruopWith = null;
            if (o.GroupWithPartners == 1 && o.GroupWithTrainer == 1)
                GruopWith = " (HGT.Price between " + search_MinBudget + " and " + search_MaxBudget + ")";
            else if (o.GroupWithPartners == 1)
                GruopWith = " HGT.WithTrainer=0 ";
            else GruopWith = " HGT.WithTrainer=1 and (HGT.Price between " + search_MinBudget + " and " + search_MaxBudget + ")";

            con1 = connect("BenefitConnectionStringName");

            selectSTR = "select distinct AGT.GroupTrainingCode, HGT.Latitude, HGT.Longitude, HGT.TrainingTime, HGT.MaxParticipants, HGT.CurrentParticipants, HGT.SportCategoryCode, HGT.Price, HGT.WithTrainer " +
                "from HistoryGroupTraining as HGT inner join ActiveGroupTraining as AGT on HGT.GroupTrainingCode = AGT.GroupTrainingCode " +
                "where " + GruopWith +
                " and( HGT.TrainingTime between '" + o.StartTime + "' and '" + o.EndTime + "') " +
                " and HGT.StatusCode<>'3' " +
                sportCategoriesStr;

            SqlCommand cmd1 = new SqlCommand(selectSTR, con1);
            SqlDataReader dr1 = cmd1.ExecuteReader(CommandBehavior.CloseConnection);
            List<HistoryGroupTraining> gtl = new List<HistoryGroupTraining>();

            while (dr1.Read())
            {
                double group_Longitude = Convert.ToSingle(dr1["Longitude"]);
                double group_Latitude = Convert.ToSingle(dr1["Latitude"]);
                double distance = distances(group_Latitude, group_Longitude, Convert.ToDouble(o.Latitude), Convert.ToDouble(o.Longitude), 'K');
                if ((distance <= search_SearchRadius))
                {
                    HistoryGroupTraining hgt = new HistoryGroupTraining();
                    hgt.Longitude = Convert.ToSingle(group_Longitude);
                    hgt.Latitude = Convert.ToSingle(group_Latitude);
                    hgt.TrainingCode = Convert.ToInt32(dr1["GroupTrainingCode"]);
                    hgt.TrainingTime = Convert.ToString(dr1["TrainingTime"]);
                    hgt.Price = Convert.ToInt32(dr1["Price"]);
                    hgt.WithTrainer = Convert.ToInt32(dr1["WithTrainer"]);
                    gtl.Add(hgt);
                }

            }
            return gtl;

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

    //השאילתה מחשבת את המרחק בין 2 קואורדינטות
    private double distances(double lat1, double lon1, double lat2, double lon2, char unit)
    {
        if ((lat1 == lat2) && (lon1 == lon2))
        {
            return 0;
        }
        else
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return (dist);
        }
    }

    private double rad2deg(double rad)
    {
        return (rad / Math.PI * 180.0);
    }

    private double deg2rad(double deg)
    {
        return (deg * Math.PI / 180.0);
    }

    //הפונקציה מכניסה חיפוש של מתאמן למערכת ובנוסף מבצעת את החיפושים בהתאם למה שהמשתמש הכניס
    public List<Result> InsertOnlineTrainee(OnlineHistoryTrainee o)
    {
        SqlConnection con;
        SqlConnection con1 = null;
        SqlConnection con2 = null;
        SqlConnection con3 = null;
        SqlCommand cmd;
        SqlCommand cmd1 = null;
        SqlCommand cmd2 = null;
        SqlCommand cmd3 = null;

        //delete all trainees/trainers/groups that are not active (end time is over)
        try
        {
            DeleteNotActive();
        }

        catch (Exception ex)
        {
            throw (ex);
        }


        try
        {
            con = connect("BenefitConnectionStringName");

            //Get onlines and check if there is one open for this trainee, if yes, delete it.
            String selectSTR = "select * from CurrentOnlineTrainee as COT inner join OnlineHistoryTrainee as OHT on COT.OnlineCode = OHT.OnlineCode where OHT.TraineeCode = '" + o.UserCode + "'";
            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            con1 = connect("BenefitConnectionStringName");
            String selectSTR1 = "";
            while (dr.Read())
            {
                int OnlineCode = Convert.ToInt32(dr["OnlineCode"]);
                //deletes previous online (need to do the function)
                selectSTR1 = "DELETE FROM CurrentOnlineTrainee WHERE OnlineCode = " + OnlineCode;
                cmd1 = new SqlCommand(selectSTR1, con1);
            }
            SqlDataReader dr1 = null;
            if (cmd1 != null)
                dr1 = cmd1.ExecuteReader(CommandBehavior.CloseConnection);

        }
        catch (Exception ex)
        {
            throw (ex);
        }

        con2 = connect("BenefitConnectionStringName");
        String pStr2 = BuildInsertOnlineHistoryTraineeCommand(o);
        cmd2 = CreateCommand(pStr2, con2);
        con3 = connect("BenefitConnectionStringName");
        try
        {
            int OnlineCode = Convert.ToInt32(cmd2.ExecuteScalar());
            String pStr3 = BuildInsertCurrentTraineeCommand(OnlineCode);
            cmd3 = CreateCommand(pStr3, con3);
            cmd3.ExecuteNonQuery();
            List<Result> Partners = null;
            List<Result> Trainers = null;
            if (o.WithPartner == 1 && o.WithTrainer == 1)
            {
                Partners = SearchPartners(o);
                Trainers = SearchTrainers(o);
                Partners.AddRange(Trainers);
                return Partners;
            }
            else if (o.WithPartner == 1)
                return SearchPartners(o);
            else if (o.WithTrainer == 1)
                return SearchTrainers(o);
            else return null;
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

    //הפונקציה מכניסה מאמן פעיל למערכת
    public void InsertOnlineTrainer(OnlineHistoryTrainer o)
    {
        SqlConnection con;
        SqlConnection con1 = null;
        SqlConnection con2 = null;
        SqlConnection con3 = null;
        SqlCommand cmd;
        SqlCommand cmd1 = null;
        SqlCommand cmd2 = null;
        SqlCommand cmd3 = null;
        try
        {
            con = connect("BenefitConnectionStringName");

            //Get onlines and check if there is one open for this trainer, if yes, delete it.
            String selectSTR = "select * from CurrentOnlineTrainer as COT inner join OnlineHistoryTrainer as OHT on COT.OnlineCode = OHT.OnlineCode where OHT.TrainerCode = '" + o.UserCode + "'";
            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            con1 = connect("BenefitConnectionStringName");
            String selectSTR1 = "";
            while (dr.Read())
            {
                int OnlineCode = Convert.ToInt32(dr["OnlineCode"]);
                //deletes previous online (need to do the function)
                selectSTR1 = "DELETE FROM CurrentOnlineTrainer WHERE OnlineCode = " + OnlineCode;
                cmd1 = new SqlCommand(selectSTR1, con1);
            }
            SqlDataReader dr1 = null;
            if (cmd1 != null)
                dr1 = cmd1.ExecuteReader(CommandBehavior.CloseConnection);

        }
        catch (Exception ex)
        {
            throw (ex);
        }
        con2 = connect("BenefitConnectionStringName");
        String pStr2 = BuildInsertOnlineHistoryTrainerCommand(o);
        cmd2 = CreateCommand(pStr2, con2);
        con3 = connect("BenefitConnectionStringName");
        try
        {
            int OnlineCode = Convert.ToInt32(cmd2.ExecuteScalar());
            String pStr3 = BuildInsertCurrentTrainerCommand(OnlineCode);
            cmd3 = CreateCommand(pStr3, con3);
            cmd3.ExecuteNonQuery();
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

    public void InsertGroupTraining(HistoryGroupTraining h)
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlConnection con1;
        SqlCommand cmd1;

        con = connect("BenefitConnectionStringName");
        try
        {
            String pStr = BuildInsertHistoryGroupTrainingCommand(h);
            cmd = CreateCommand(pStr, con);
            int HistoryGroupTrainingCode = Convert.ToInt32(cmd.ExecuteScalar());
            con1 = connect("BenefitConnectionStringName");
            String pStr1 = BuildInsertActiveGroupTrainingCommand(HistoryGroupTrainingCode);
            cmd1 = CreateCommand(pStr1, con1);
            cmd1.ExecuteNonQuery();
            int AddParticipantsNum = 0;
            if (h.WithTrainer == 0) AddParticipantsNum = 1;
            JoinGroup(h.CreatorCode, HistoryGroupTrainingCode, AddParticipantsNum);
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


    // הפונקציה רצה על טבלת האונליין ומוחקת את מי שזמן סיום הפעילות שלו חלף
    private void DeleteNotActive()
    {

        SqlConnection con = null;

        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");

            //Get trainee's details that needed for the search
            String selectSTR = "DELETE FROM CurrentOnlineTrainee  WHERE OnlineCode in (select OHT.OnlineCode" +
                " from OnlineHistoryTrainee as OHT inner join CurrentOnlineTrainee as CO on OHT.OnlineCode = CO.OnlineCode" +
                " where DATEDIFF(second, OHT.EndTime, getdate()) > 0)" +
                " DELETE FROM CurrentOnlineTrainer WHERE OnlineCode in (select OHT.OnlineCode" +
                " from OnlineHistoryTrainer as OHT inner join CurrentOnlineTrainer as CO on OHT.OnlineCode = CO.OnlineCode" +
                " where DATEDIFF(second, OHT.EndTime, getdate()) > 0)" +
                " DELETE FROM ActiveGroupTraining WHERE GroupTrainingCode in (select AGT.GroupTrainingCode" +
                " from ActiveGroupTraining as AGT inner join HistoryGroupTraining as HGT on AGT.GroupTrainingCode = HGT.GroupTrainingCode" +
                " where DATEDIFF(second, HGT.TrainingTime, getdate()) > 0)";

            cmd = new SqlCommand(selectSTR, con);
            cmd.ExecuteNonQuery();
            //    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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

    public void UpdateToken(string Token, int UserCode)
    {

        SqlConnection con = null;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");
            //Get trainee's details that needed for the search
            String selectSTR = "Update Users set Token='" + Token + "' where Users.UserCode=" + UserCode;
            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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

    public List<Trainee> GetLazyTrainees()
    {

        SqlConnection con = null;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");

            //Get trainee's details that needed for the search
            String selectSTR = "SELECT U.Token, U.FirstName " +
                " FROM Trainees as T inner join Users as U on T.TraineeCode = U.UserCode" +
                " where (datediff(day, U.SignUpDate, getdate()) >= 7) AND (T.TraineeCode NOT IN" +
                " (SELECT CTS.ReceiverCode" +
                " from CoupleTraining as CT inner join CoupleTrainingSuggestions as CTS on CT.SuggestionCode = CTS.SuggestionCode" +
                " where datediff(day, CT.TrainingTime, getdate()) <= 7))" +
                " AND(T.TraineeCode NOT IN" +
                " (SELECT CTS.SenderCode" +
                " from CoupleTraining as CT inner join CoupleTrainingSuggestions as CTS on CT.SuggestionCode = CTS.SuggestionCode" +
                " where datediff(day, CT.TrainingTime, getdate()) <= 7))" +
                " AND(T.TraineeCode NOT IN" +
                " (SELECT GP.UserCode" +
                " FROM HistoryGroupTraining as HGT inner join GroupParticipants as GP on HGT.GroupTrainingCode = GP.GroupTrainingCode" +
                " where(datediff(day, HGT.TrainingTime, getdate()) <= 7)  and(HGT.StatusCode <> 2)))";
            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<Trainee> tl = new List<Trainee>();

            //****returns list with trainee code and first name only****//

            while (dr.Read())
            {
                Trainee t = new Trainee();
                t.FirstName = Convert.ToString(dr["FirstName"]);
                t.Token = Convert.ToString(dr["Token"]);


                tl.Add(t);
            }

            return tl;
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

    public List<Trainer> GetLazyTrainers()
    {

        SqlConnection con = null;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");

            //Get trainee's details that needed for the search
            String selectSTR = "SELECT U.Token, U.FirstName " +
                " FROM Trainers as T inner join Users as U on T.TrainerCode = U.UserCode" +
                " where (datediff(day, U.SignUpDate, getdate()) >= 7) AND (T.TrainerCode NOT IN" +
                " (SELECT CTS.ReceiverCode" +
                " from CoupleTraining as CT inner join CoupleTrainingSuggestions as CTS on CT.SuggestionCode = CTS.SuggestionCode" +
                " where datediff(day, CT.TrainingTime, getdate()) <= 7))" +
                " AND(T.TrainerCode NOT IN" +
                " (SELECT HGT.CreatorCode" +
                " FROM HistoryGroupTraining as HGT inner join Trainers as T on HGT.CreatorCode=T.TrainerCode" +
                " where(datediff(day, HGT.TrainingTime, getdate()) <= 7) and(HGT.StatusCode <> 2))" +
                ")";

            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<Trainer> tl = new List<Trainer>();

            //****returns list with trainee code and first name only****//

            while (dr.Read())
            {
                Trainer t = new Trainer();
                t.FirstName = Convert.ToString(dr["FirstName"]);
                t.Token = Convert.ToString(dr["Token"]);


                tl.Add(t);
            }

            return tl;
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

    public void JoinGroup(int UserCode, int GroupTrainingCode, int AddParticipantsNum)
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
            String pStr = BuildInsertGroupParticipantsCommand(UserCode, GroupTrainingCode);
            cmd = CreateCommand(pStr, con);
            cmd.ExecuteNonQuery();
            UpdateNumOfParticipants(AddParticipantsNum, GroupTrainingCode);
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

    public List<PrefferedDay> GetPrefferedTrainingDay()
    {

        SqlConnection con = null;
        SqlConnection con2 = null;
        SqlCommand cmd;
        SqlCommand cmd2;

        try
        {
            con = connect("BenefitConnectionStringName");

            // השאילתא מחזירה את כל המתאמנים שהתאמנו יותר מפעמיים באותו יום
            String selectSTR = "SELECT distinct RES.TraineeCode, RES.Token " +
                "FROM(Select  T.TraineeCode, U.Token, " +
                "case when  DATENAME(dw, CT.TrainingTime) = 'Sunday' then count(T.TraineeCode) " +
                "when  DATENAME(dw, CT.TrainingTime) = 'Monday' then count(T.TraineeCode) " +
                "when  DATENAME(dw, CT.TrainingTime) = 'Tuesday' then count(T.TraineeCode) " +
                "when  DATENAME(dw, CT.TrainingTime) = 'Wednesday'  then count(T.TraineeCode) " +
                "when  DATENAME(dw, CT.TrainingTime) = 'Thursday' then count(T.TraineeCode) " +
                "when  DATENAME(dw, CT.TrainingTime) = 'Friday' then count(T.TraineeCode) " +
                "when  DATENAME(dw, CT.TrainingTime) = 'Saturday' then count(T.TraineeCode) " +
                "end as 'NumOfTrainings' " +
                "from Trainees as T inner join CoupleTrainingSuggestions as CTS on T.TraineeCode = CTS.SenderCode or  T.TraineeCode = CTS.ReceiverCode inner join CoupleTraining as CT ON CT.SuggestionCode = CTS.SuggestionCode inner join  Users as U  on U.UserCode = T.TraineeCode " +
                "GROUP BY  T.TraineeCode, DATENAME(dw, CT.TrainingTime), U.Token)AS RES " +
                "GROUP BY RES.TraineeCode,  RES.NumOfTrainings, RES.Token " +
                "HAVING RES.NumOfTrainings >= 2 ";

            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<User> ul = new List<User>();
            List<PrefferedDay> ol = new List<PrefferedDay>();

            while (dr.Read())
            {
                User User = new User();
                User.UserCode = Convert.ToInt32(dr["TraineeCode"]);
                User.Token = Convert.ToString(dr["Token"]);
                ul.Add(User);
            }

            con2 = connect("BenefitConnectionStringName");
            // מחזירה עבור כל מתאמן כמה פעמים הוא התאמן בכל יום 
            String SelectSTR2 = "SELECT * FROM(Select distinct T.TraineeCode, DATENAME(dw, CT.TrainingTime) as 'DayName', " +
                "case " +
                "when  DATENAME(dw, CT.TrainingTime) = 'Sunday' then count(T.TraineeCode) " +
                "when  DATENAME(dw, CT.TrainingTime) = 'Monday' then count(T.TraineeCode) " +
                "when  DATENAME(dw, CT.TrainingTime) = 'Tuesday' then count(T.TraineeCode) " +
                "when  DATENAME(dw, CT.TrainingTime) = 'Wednesday'  then count(T.TraineeCode) " +
                "when  DATENAME(dw, CT.TrainingTime) = 'Thursday' then count(T.TraineeCode) " +
                "when  DATENAME(dw, CT.TrainingTime) = 'Friday' then count(T.TraineeCode) " +
                "when  DATENAME(dw, CT.TrainingTime) = 'Saturday' then count(T.TraineeCode) " +
                "end as 'NumOfTrainings' " +
                "from Trainees as T inner join CoupleTrainingSuggestions as CTS on T.TraineeCode = CTS.SenderCode or  T.TraineeCode = CTS.ReceiverCode inner join CoupleTraining as CT ON CT.SuggestionCode = CTS.SuggestionCode " +
                "GROUP BY  T.TraineeCode, DATENAME(dw, CT.TrainingTime))AS RES " +
                "GROUP BY RES.TraineeCode, RES.DayName, RES.NumOfTrainings " +
                "HAVING RES.NumOfTrainings >= 2";

            cmd2 = new SqlCommand(SelectSTR2, con2);
            SqlDataReader dr2 = cmd2.ExecuteReader(CommandBehavior.CloseConnection);
            List<PrefferedDay> returnList = new List<PrefferedDay>();

            while (dr2.Read())
            {
                PrefferedDay pd = new PrefferedDay();
                pd.UserCode = Convert.ToInt32(dr2["TraineeCode"]);
                pd.DayName = Convert.ToString(dr2["DayName"]);
                pd.NumOfTrainings = Convert.ToInt32(dr2["NumOfTrainings"]);
                ol.Add(pd);
                
            }
            for (int i = 0; i < ul.Count; i++)
            {
                int numberOfTrainings = 0;
                PrefferedDay p = new PrefferedDay();
                for (int j = 0; j < ol.Count; j++)
                {
                    
                    if (ol[j].UserCode == ul[i].UserCode)
                    {
                        
                        if (numberOfTrainings < ol[j].NumOfTrainings)
                        {
                            numberOfTrainings = ol[j].NumOfTrainings;

                            p.DayName = ol[j].DayName;
                            p.UserCode = ol[j].UserCode;
                        }
                    }
                }
                numberOfTrainings = 0;
                returnList.Add(p);
            }
            return returnList;
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
    //this function gets num=1 if added a new participants, num=-1 if deleting one participant
    private void UpdateNumOfParticipants(int Num, int GroupTrainingCode)
    {

        SqlConnection con = null;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");
            //update num
            String selectSTR = "Update HistoryGroupTraining set CurrentParticipants=CurrentParticipants+'" + Num + "' where GroupTrainingCode='" +GroupTrainingCode+
                "'  Update HistoryGroupTraining set StatusCode=3 where CurrentParticipants=MaxParticipants";
            cmd = new SqlCommand(selectSTR, con);
            //int CurrentParticipants = Convert.ToInt32(cmd.ExecuteScalar());
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

  public string SendSuggestion(int SenderCode, int ReceiverCode)
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
			String pStr = BuildInsertSuggestionCommand(SenderCode, ReceiverCode);
			cmd = CreateCommand(pStr, con);
			cmd.ExecuteNonQuery();
			return GetToken(ReceiverCode);
		
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

    public void ReplySuggestion(int SuggestionCode, bool Reply)
    {

        SqlConnection con=null;
        SqlCommand cmd;
        int StatusCode=0;
        if (Reply == true) StatusCode = 5;
        else StatusCode = 6;

        try
        {
            con = connect("BenefitConnectionStringName");
            String selectSTR = "Update CoupleTrainingSuggestions set StatusCode='" + StatusCode + "' where SuggestionCode='" + SuggestionCode+"'";
            cmd = new SqlCommand(selectSTR, con);
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


    public string GetToken(int UserCode)
	{

		SqlConnection con = null;
		string token = "";

		try
		{
			con = connect("BenefitConnectionStringName");

			String selectSTR = "select Users.Token from Users where Users.UserCode='" + UserCode+ "'";
			SqlCommand cmd = new SqlCommand(selectSTR, con);

			SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
			if (dr.Read())
			{

				token=  Convert.ToString(dr["Token"]);
				
				return token;
			}
			else
			{
			
				return null;
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

    //get pending suggestions - Sender=true if the usercode is the sender, false if he is receivrr
    public List<CoupleTrainingSuggestion> GetSuggestions(int UserCode,bool Sender, bool IsApproved)
    {
        SqlConnection con = null;
        SqlCommand cmd;
        string SenderStr = null;
        if (Sender) SenderStr = "SenderCode"; else SenderStr = "ReceiverCode";
        string IsApprovedStr = null;
        if (IsApproved) IsApprovedStr = "5"; else IsApprovedStr = "4";
        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "select CTS.SuggestionCode" +
                " from CoupleTrainingSuggestions CTS inner" +
                " join Users U on CTS.SenderCode = U.UserCode" +
                " where CTS."+ SenderStr + "="+UserCode+" and CTS.StatusCode = "+ IsApprovedStr;

            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<CoupleTrainingSuggestion> ctsl = new List<CoupleTrainingSuggestion>();
                
      
            while (dr.Read())
            {
                CoupleTrainingSuggestion cts = new CoupleTrainingSuggestion();
                cts.SuggestionCode = Convert.ToInt32(dr["SuggestionCode"]);
                ctsl.Add(cts);
            }

            return ctsl;
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

  


    public List<Result> GetSuggestionDetails(int SuggestionCode)
    {
        SqlConnection con = null;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "select CTS.SenderCode, U.FirstName, U.LastName,datediff(year, U.DateOfBirth, getdate()) as Age, U.Picture,OHT.Latitude,OHT.Longitude" +
                " from CoupleTrainingSuggestions CTS inner " +
                "join Users U on CTS.SenderCode = U.UserCode inner " +
                "join OnlineHistoryTrainee as OHT on OHT.TraineeCode = CTS.SenderCode " +
                " inner join CurrentOnlineTrainee as C on C.OnlineCode=OHT.OnlineCode" +
                " where CTS.SuggestionCode = "+SuggestionCode+" and CTS.StatusCode = 4";

            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<Result> rl = new List<Result>();


            while (dr.Read())
            {
                Result r = new Result();
                r.UserCode = Convert.ToInt32(dr["SenderCode"]);
                r.FirstName = Convert.ToString(dr["FirstName"]);
                r.LastName = Convert.ToString(dr["LastName"]);
                r.Age = Convert.ToInt32(dr["Age"]);
                r.Picture = Convert.ToString(dr["Picture"]);
                string _lat = Convert.ToString(dr["Latitude"]);
                r.Latitude = float.Parse(_lat);
                string _long = Convert.ToString(dr["Longitude"]);
                r.Longitude = float.Parse(_long);
                rl.Add(r);
            }

            return rl;
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

    public List<CoupleTraining> GetFutureTrainings(int UserCode)
    {
        SqlConnection con = null;
        SqlCommand cmd;
        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "select CT.CoupleTrainingCode, CT.Latitude,CT.Longitude,CT.TrainingTime,CT.WithTrainer " +
                "from CoupleTraining as CT inner join CoupleTrainingSuggestions AS CTS on CT.SuggestionCode = CTS.SuggestionCode " +
                "where(CTS.SenderCode = 2 or CTS.ReceiverCode = 2) and(datediff(hour, getdate(), CT.TrainingTime) > 0)";

            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        
            List<CoupleTraining> ct = new List<CoupleTraining>();


            while (dr.Read())
            {
                CoupleTraining c = new CoupleTraining();
                c.TrainingCode= Convert.ToInt32(dr["CoupleTrainingCode"]);
                c.TrainingTime = Convert.ToString(dr["TrainingTime"]);
                string _lat = Convert.ToString(dr["Latitude"]);
                c.Latitude = float.Parse(_lat);
                string _long = Convert.ToString(dr["Longitude"]);
                c.Longitude = float.Parse(_long);
                c.WithTrainer= Convert.ToInt32(dr["WithTrainer"]);
                
                ct.Add(c);
            }

            return ct;
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

        sb.AppendFormat("Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},{8},{9}, {10})", u.Email, u.FirstName, u.LastName, u.Password, u.Gender, u.DateOfBirth, u.Picture, u.SearchRadius.ToString(), u.IsTrainer.ToString(), u.Rate.ToString(), "getdate()");
        String prefix = "INSERT INTO Users (Email, FirstName, LastName, Password, Gender, DateOfBirth, Picture, SearchRadius, IsTrainer, Rate, SignUpDate) output INSERTED.UserCode ";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertTraineeCommand(int UserCode, Trainee t)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values({0},{1},{2},'{3}','{4}', {5}, {6})", UserCode.ToString(), t.MinBudget.ToString(), t.MaxBudget.ToString(), t.PartnerGender, t.TrainerGender, t.MinPartnerAge.ToString(), t.MaxPartnerAge.ToString());
        String prefix = "INSERT INTO Trainees (TraineeCode, MinBudget, MaxBudget, PartnerGender, TrainerGender, MinPartnerAge, MaxPartnerAge) ";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertTrainerCommand(int UserCode, Trainer t)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values({0},{1})", UserCode.ToString(), t.PersonalTrainingPrice.ToString());
        String prefix = "INSERT INTO Trainers (TrainerCode, PersonalTrainingPrice) ";
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
        sb.AppendFormat("Values({0},{1},{2},{3},'{4}', '{5}', {6}, {7}, {8}, {9} )", o.UserCode.ToString(), "getdate()" , o.Latitude.ToString(), o.Longitude.ToString(), o.StartTime, o.EndTime, o.WithTrainer.ToString(), o.WithPartner.ToString(), o.GroupWithTrainer.ToString(), o.GroupWithPartners.ToString());
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

        sb.AppendFormat("Values({0},{1},{2},{3},'{4}', '{5}')", o.UserCode.ToString(), "getdate()", o.Latitude.ToString(), o.Longitude.ToString(), o.StartTime, o.EndTime);
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

    private String BuildInsertHistoryGroupTrainingCommand(HistoryGroupTraining h)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values('{0}',{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})",h.TrainingTime, h.Latitude.ToString(), h.Longitude.ToString(), h.WithTrainer.ToString(), h.CreatorCode.ToString(), h.MinParticipants.ToString(), h.MaxParticipants.ToString(), h.CurrentParticipants.ToString(), h.StatusCode.ToString(), h.SportCategoryCode.ToString(), h.Price.ToString());
        String prefix = "INSERT INTO HistoryGroupTraining (TrainingTime, Latitude, Longitude, WithTrainer, CreatorCode, MinParticipants, MaxParticipants, CurrentParticipants, StatusCode,SportCategoryCode, Price ) output INSERTED.GroupTrainingCode  ";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertActiveGroupTrainingCommand(int HistoryGroupTrainingCode)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values({0})", HistoryGroupTrainingCode.ToString());
        String prefix = "INSERT INTO ActiveGroupTraining (GroupTrainingCode) ";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertGroupParticipantsCommand(int UserCode, int GroupTrainingCode)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values({0},{1},'{2}')", UserCode.ToString(), GroupTrainingCode.ToString(), '1');
        String prefix = "INSERT INTO GroupParticipants (UserCode, GroupTrainingCode, StatusCode) ";
        command = prefix + sb.ToString();
        return command;
    }



	private String BuildInsertSuggestionCommand(int SenderCode, int ReceiverCode)
	{
		String command;
		StringBuilder sb = new StringBuilder();

		sb.AppendFormat("Values({0},{1},{2}, {3})", SenderCode.ToString(), ReceiverCode.ToString(), "getdate()", 4);
		String prefix = "INSERT INTO CoupleTrainingSuggestions (SenderCode, ReceiverCode, SendingTime, StatusCode) ";
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

