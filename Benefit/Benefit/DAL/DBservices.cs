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

        // read the connection string from the configuration file
        string pStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
        SqlConnection con = new SqlConnection(pStr);
        con.Open();
        return con;
    }

    //--------------------------------------------------------------------------------------------------
    // This method inserts a car to the cars table 
    //--------------------------------------------------------------------------------------------------

    public int insert(Person person)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("tinder_ConnectionStringName"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String pStr = BuildInsertCommand(person);      // helper method to build the insert string
        cmd = CreateCommand(pStr, con);             // create the command

        try
        {
            int person_id = Convert.ToInt32(cmd.ExecuteScalar());

            for (int i = 0; i < person.Hobbies.Length; i++)
            {
                String str = BuildInsertCommand2(person.Hobbies[i], person_id);
                cmd = CreateCommand(str, con);
                int numEffected = cmd.ExecuteNonQuery();// execute the command
            }

            return 1;
            //return person_id;

        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    public int editDetails(Person person)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("tinder_ConnectionStringName"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String pStr = BuildUpdateCommand(person);      // helper method to build the insert string
        cmd = CreateCommand(pStr, con);             // create the command



        try
        {
            int person_id = Convert.ToInt32(cmd.ExecuteScalar());
            String pStr1 = BuildDeleteCommand(person_id);
            cmd = CreateCommand(pStr1, con);
            int numEffected2 = cmd.ExecuteNonQuery();

            for (int i = 0; i < person.Hobbies.Length; i++)
            {
                String str = BuildInsertCommand2(person.Hobbies[i], person_id);
                cmd = CreateCommand(str, con);
                int numEffected = cmd.ExecuteNonQuery();// execute the command
            }

            return 1;
            //return person_id;

        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    public List<Person> get_person(string conString, string tableName)
    {

        SqlConnection con = null;
        SqlConnection con1 = null;
        List<Person> pl = new List<Person>();

        try
        {
            con = connect(conString); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT * FROM " + tableName;
            SqlCommand cmd = new SqlCommand(selectSTR, con);


            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            while (dr.Read())
            {   // Read till the end of the data into a row
                Person p = new Person();
                p.Id = Convert.ToInt32(dr["Id"]);
                p.Name = Convert.ToString(dr["Name"]);
                p.FamilyName = Convert.ToString(dr["FamilyName"]);
                p.Gender = Convert.ToString(dr["Gender"]);
                p.Age = Convert.ToDouble(dr["Age"]);
                p.Height = Convert.ToDouble(dr["Height"]);
                p.Image = Convert.ToString(dr["Image"]);
                p.Address = Convert.ToString(dr["Address"]);
                p.IsActive = Convert.ToInt32(dr["IsActive"]);
                p.IsPremium = Convert.ToInt32(dr["IsPremium"]);
                p.Phone = Convert.ToString(dr["Phone"]);


                con1 = connect(conString);
                String selectSTR1 = "SELECT * FROM Person_Hobbies1 where Person_Hobbies1.person_id=" + p.Id;

                SqlCommand cmd1 = new SqlCommand(selectSTR1, con1);
                SqlDataReader dr1 = cmd1.ExecuteReader(CommandBehavior.CloseConnection);


                List<int> h = new List<int>();
                int[] hl = null;
                while (dr1.Read())
                {   // Read till the end of the data into a row
                    h.Add(Convert.ToInt32(dr1["hobbie_num"]));

                    // get a reader
                }
                hl = h.ToArray();
                p.Hobbies = hl;
                pl.Add(p);

            }
            return pl;

        }
        catch (Exception ex)
        {
            // write to log
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

    public Person GetDetails(string conString, string tableName, string email)
    {

        SqlConnection con = null;
        SqlConnection con1 = null;

        try
        {
            con = connect(conString); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT * FROM " + tableName + " WHERE " + tableName + ".Email = '" + email + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);


            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
            Person p = new Person();
            while (dr.Read())
            {   // Read till the end of the data into a row

                p.Id = Convert.ToInt32(dr["Id"]);
                p.Name = Convert.ToString(dr["Name"]);
                p.FamilyName = Convert.ToString(dr["FamilyName"]);
                p.Gender = Convert.ToString(dr["Gender"]);
                p.Age = Convert.ToDouble(dr["Age"]);
                p.Height = Convert.ToDouble(dr["Height"]);
                p.Image = Convert.ToString(dr["Image"]);
                p.Address = Convert.ToString(dr["Address"]);
                p.IsActive = Convert.ToInt32(dr["IsActive"]);
                p.IsPremium = Convert.ToInt32(dr["IsPremium"]);
                p.Phone = Convert.ToString(dr["Phone"]);
                p.Email = Convert.ToString(dr["Email"]);
                p.Password = Convert.ToString(dr["Password"]);

                con1 = connect(conString);
                String selectSTR1 = "SELECT * FROM Person_Hobbies1 where Person_Hobbies1.person_id=" + p.Id;

                SqlCommand cmd1 = new SqlCommand(selectSTR1, con1);
                SqlDataReader dr1 = cmd1.ExecuteReader(CommandBehavior.CloseConnection);


                List<int> h = new List<int>();
                int[] hl = null;
                while (dr1.Read())
                {   // Read till the end of the data into a row
                    h.Add(Convert.ToInt32(dr1["hobbie_num"]));

                    // get a reader
                }
                hl = h.ToArray();
                p.Hobbies = hl;


            }
            return p;

        }
        catch (Exception ex)
        {
            // write to log
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

    public bool checkEmail(string conString, string tableName, string email)
    {

        SqlConnection con = null;

        try
        {
            con = connect(conString); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "select " + tableName + ".Password from " + tableName + " where " + tableName + ".Email='" + email + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

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
            // write to log
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

    public int Active(int isActive, int PersonId)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("tinder_ConnectionStringName"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        String pStr = BuildInsertCommand3(isActive, PersonId);      // helper method to build the insert string
        cmd = CreateCommand(pStr, con);             // create the command

        try
        {
            int numAffected = cmd.ExecuteNonQuery();
            return numAffected;


        }
        catch (Exception ex)
        {
            return 0;
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    public List<Hobbies> Read_hobbies(string conString, string tableName)
    {

        SqlConnection con = null;
        List<Hobbies> hl = new List<Hobbies>();
        try
        {
            con = connect(conString); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "SELECT * FROM " + tableName;
            SqlCommand cmd = new SqlCommand(selectSTR, con);


            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            while (dr.Read())
            {   // Read till the end of the data into a row
                Hobbies h = new Hobbies();

                h.Num = Convert.ToInt32(dr["num"]);
                h.Description = (dr["description"]).ToString();
                hl.Add(h);
            }

            return hl;
        }
        catch (Exception ex)
        {
            // write to log
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

    public bool login(string Email, string Password)
    {

        SqlConnection con = null;

        try
        {
            con = connect("tinder_ConnectionStringName"); // create a connection to the database using the connection String defined in the web config file

            String selectSTR = "select PersonTbl.Password from PersonTbl where PersonTbl.Email='" + Email + "' and PersonTbl.Password='" + Password + "' ";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            // get a reader
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

            if (dr.Read()) return true;
            else return false;

        }
        catch (Exception ex)
        {
            // write to log
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
    private String BuildInsertCommand(Person person)
    {
        String command;
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values('{0}','{1}','{2}',{3},{4},'{5}','{6}',{7},{8},'{9}', '{10}', '{11}')", person.Name, person.FamilyName, person.Gender, person.Age.ToString(), person.Height.ToString(), person.Address, person.Image, person.IsActive, person.IsPremium, (person.Phone).ToString(), person.Email, person.Password);
        String prefix = "INSERT INTO PersonTbl (Name, FamilyName, Gender, Age, Height , Address, Image, IsActive, IsPremium, Phone, Email, Password) output INSERTED.Id ";

        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertCommand2(int hobbie, int person_id)
    {
        String command;
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        sb.AppendFormat("Values({0}, {1})", hobbie, person_id);
        String prefix = "INSERT INTO Person_Hobbies1 (hobbie_num, person_id) ";

        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertCommand3(int isActive, int PersonId)
    {
        String command;
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        //sb.AppendFormat("Values({0})", isActive);
        String prefix = "UPDATE PersonTbl SET IsActive = " + isActive + " WHERE PersonTbl.Id =" + PersonId + " ";

        command = prefix + sb.ToString();
        return command;
    }

    private String BuildUpdateCommand(Person p)
    {
        String command;
        StringBuilder sb = new StringBuilder();
        // use a string builder to create the dynamic string
        //sb.AppendFormat("Values('{0}','{1}','{2}',{3},{4},'{5}','{6}',{7},{8},'{9}', '{10}')", person.Name, person.FamilyName, person.Gender, person.Age.ToString(), person.Height.ToString(), person.Address, person.Image, person.IsActive, person.IsPremium, (person.Phone).ToString(), person.Password);
        //String prefix1 = "INSERT INTO PersonTbl (Name, FamilyName, Gender, Age, Height , Address, Image, IsActive, IsPremium, Phone, Password) output UPDATED.Id ";
        string prefix = "UPDATE PersonTbl SET Name = '" + p.Name + "', FamilyName = '" + p.FamilyName + "', Address ='" + p.Address + "' ,  Gender = '" + p.Gender + "' , Age = " + p.Age + ", Height = " + p.Height + " ,Image = '" + p.Image + "' , IsActive = " + p.IsActive + ", IsPremium = " + p.IsPremium + " , Phone = '" + p.Phone + "' , Password ='" + p.Password + "' output INSERTED.Id WHERE PersonTbl.Email = '" + p.Email + "'";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildDeleteCommand(int Id)
    {

        String prefix = "DELETE FROM Person_Hobbies1 WHERE person_id=" + Id;
       
        return prefix;
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
