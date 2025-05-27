using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Avalonia_Hotel;

public class GestioneDati
{
    private MySqlConnection con;
    //Costruttore
    public GestioneDati()
    {
        string s = "database=hotel;host=localhost;port=3306;user=root;pwd=root";
        con = new MySqlConnection(s);
        con.Open();
    }

    public List<DateTime> RecuperaCalendarioDiUnaCamera(int id)
    {
        List<DateTime> ris = new List<DateTime>();
        string query = "SELECT datacheckin,datacheckout " +
                       "FROM prenotazioni INNER JOIN dettagli_prenotazioni ON prenotazioni.id=dettagli_prenotazioni.idPrenotazione " +
                       "WHERE idCamera = @id";
        MySqlCommand cmd = new MySqlCommand(query,con);
        cmd.Parameters.AddWithValue("@id",id);
        MySqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            DateTime check_in = (DateTime) reader["dataCheckIn"];
            DateTime check_out = (DateTime) reader["dataCheckOut"];
            for (DateTime d = check_in; d <= check_out; d=d.AddDays(1))
            {
                ris.Add(d);
            }
        }
        reader.Close();
        return ris;
    }
    public DateTime RecuperaCheckInDiUnaPrenotazione(int id)
    {
        DateTime check_in=new DateTime();
        string query = "SELECT dataCheckIn FROM prenotazioni WHERE id=@id";
        MySqlCommand cmd = new MySqlCommand(query,con);
        cmd.Parameters.AddWithValue("@id",id);
        MySqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            check_in = (DateTime) reader["dataCheckIn"];
           
        }
        reader.Close();
        return check_in;
    }
    public DateTime RecuperaCheckOutDiUnaPrenotazione(int id)
    {
        DateTime check_out=new DateTime();
        string query = "SELECT dataCheckOut FROM prenotazioni WHERE id=@id";
        MySqlCommand cmd = new MySqlCommand(query,con);
        cmd.Parameters.AddWithValue("@id",id);
        MySqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            check_out = (DateTime) reader["dataCheckOut"];
           
        }
        reader.Close();
        return check_out;
    }
    public List<Prenotazione> RecuperaTuttePrenotazioni()
    {
        List<Prenotazione> ris = new List<Prenotazione>();
        string query = "SELECT * FROM prenotazioni ORDER BY id";
        MySqlCommand cmd = new MySqlCommand(query,con);
        MySqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Prenotazione p = new Prenotazione();
            p.id = (int)reader["id"];
            p.nomeCliente = (string)reader["nomeCliente"];
            p.cognomeCliente = (string)reader["cognomeCliente"];
            p.sesso = (string)reader["sesso"];
            p.dataCheckIn = new DateOnly(((DateTime)reader["dataCheckIn"]).Year,
                ((DateTime)reader["dataCheckIn"]).Month, ((DateTime)reader["dataCheckIn"]).Day);
            p.dataCheckOut = new DateOnly(((DateTime)reader["dataCheckOut"]).Year,
                ((DateTime)reader["dataCheckOut"]).Month, ((DateTime)reader["dataCheckOut"]).Day);
            p.numPersone = (int)reader["numPersone"];
            p.telefono = (string)reader["telefono"];
            
            ris.Add(p);
        }
        reader.Close(); //reader è unico, deve chiudere quando finisce
        return ris;
    }
    
    public List<DettagliPrenotazione> RecuperaTuttiDettagliDiUnaPrenotazione(int id)
    {
        List<DettagliPrenotazione> ris = new List<DettagliPrenotazione>();
        string query = "SELECT * FROM dettagli_prenotazioni WHERE idPrenotazione=@idPrenotazione ORDER BY idCamera";
        MySqlCommand cmd = new MySqlCommand(query,con);
        cmd.Parameters.AddWithValue("@idPrenotazione",id);//Parametro 
        MySqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            DettagliPrenotazione d = new DettagliPrenotazione();
            d.idPrenotazione = (int)reader["idPrenotazione"];
            d.idCamera = (int)reader["idCamera"];
            ris.Add(d);
        }
        reader.Close(); //reader è unico, deve chiudere quando finisce
        return ris;
    }
    
    public List<Camera> RecuperaTutteCamere()
         {
             List<Camera> ris = new List<Camera>();
             string query = "SELECT * FROM camere ";
             MySqlCommand cmd = new MySqlCommand(query,con);
             MySqlDataReader reader = cmd.ExecuteReader();
             while (reader.Read())
             {
                 Camera c = new Camera();
                 c.id = (int)reader["id"];
                 c.tipo = (string)reader["tipo"];
                 c.costo = (double)reader["costo"];
                 ris.Add(c);
             }
             reader.Close(); //reader è unico, deve chiudere quando finisce
             return ris;
         }
    public List<Camera> RecuperaTutteCamereDisponibili(List<DateTime> date)
    {
        List<Camera> ris = new List<Camera>();
        string query = "SELECT id, tipo, costo " +
                       "FROM camere  WHERE id NOT IN (" +
                                "SELECT idCamera    " +
                                "FROM dettagli_prenotazioni " +
                                "INNER JOIN prenotazioni ON dettagli_prenotazioni.idPrenotazione = prenotazioni.id " +
                                "WHERE dataCheckIn BETWEEN @dataInizio AND @dataFine " +
                                "OR dataCheckOut BETWEEN @dataInizio AND @dataFine " +
                                "OR (dataCheckIn <= @dataInizio AND dataCheckOut >= @dataFine));";
        MySqlCommand cmd = new MySqlCommand(query,con);
        cmd.Parameters.AddWithValue("@dataInizio",date.First());
        cmd.Parameters.AddWithValue("@dataFine",date.Last());
        MySqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Camera c = new Camera();
            c.id = (int)reader["id"];
            c.tipo = (string)reader["tipo"];
            c.costo = (double)reader["costo"];
            ris.Add(c);
        }
        reader.Close(); //reader è unico, deve chiudere quando finisce
        return ris;
    }

    public string RecuperaIdDiUnaPrenotazione(string nome,string cognome)
    {
        int id = -1;
        string query = "SELECT id FROM prenotazioni WHERE " +
                       "nomeCliente = @nome AND " +
                       "cognomeCliente = @cognome ";
        MySqlCommand cmd = new MySqlCommand(query,con);
        cmd.Parameters.AddWithValue("@nome",nome);
        cmd.Parameters.AddWithValue("@cognome",cognome);
        MySqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            id = (int)reader["id"];
        }
        reader.Close(); //reader è unico, deve chiudere quando finisce
        return id.ToString();
    }
    public string CrearePrenotazione(string nome,string cognome, string sesso, string dataCheckIn, string dataCheckOut, string numPersone, string telefono)
    {
        string query = "INSERT INTO `hotel`.`prenotazioni` " +
                       "(`nomeCliente`, `cognomeCliente`, `sesso`," +
                       " `dataCheckIn`, `dataCheckOut`, `numPersone`, `telefono`) " +
                       "VALUES " +
                       "(@nome, @cognome,@sesso, @dataCheckIn, @dataCheckOut, @numPersone, @telefono)";
        MySqlCommand cmd = new MySqlCommand(query,con);
        cmd.Parameters.AddWithValue("@nome",nome);
        cmd.Parameters.AddWithValue("@cognome",cognome);
        cmd.Parameters.AddWithValue("@sesso",sesso);
        cmd.Parameters.AddWithValue("@dataCheckIn",dataCheckIn);
        cmd.Parameters.AddWithValue("@dataCheckOut",dataCheckOut);
        cmd.Parameters.AddWithValue("@numPersone",numPersone);
        cmd.Parameters.AddWithValue("@telefono",telefono);
        string errore = "operazione effettuato";
        try
        {
            cmd.ExecuteNonQuery();
        }
        catch(Exception e)
        {
            errore = e.Message;
        }
        return errore;
    }
    
    public string CreareDettagliPrenotazione(string idPrenotazione, string idCamera )
    {
        string query = "INSERT INTO `hotel`.`dettagli_prenotazioni` " +
                       "(`idPrenotazione`, `idCamera`) VALUES (@idPrenotazione, @idCamera);";
        MySqlCommand cmd = new MySqlCommand(query,con);
        cmd.Parameters.AddWithValue("@idPrenotazione",idPrenotazione);
        cmd.Parameters.AddWithValue("@idCamera",idCamera);
        string errore = "operazione effettuato";
        try
        {
            cmd.ExecuteNonQuery();
        }
        catch(Exception e)
        {
            errore = e.Message;
        }
        return errore;
    }
    public string AggiornaPrenotazione(string id,string nome,string cognome, string sesso, string numPersone, string telefono)
    {
        string query = "UPDATE `hotel`.`prenotazioni` SET `nomeCliente`=@nome, " +
                       "`cognomeCliente`=@cognome, `sesso`=@sesso, `numPersone`=@numPersone," +
                       "`telefono`=@telefono WHERE  `id`=@id;" ;
        MySqlCommand cmd = new MySqlCommand(query,con);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@nome",nome);
        cmd.Parameters.AddWithValue("@cognome",cognome);
        cmd.Parameters.AddWithValue("@sesso",sesso);
        cmd.Parameters.AddWithValue("@numPersone",numPersone);
        cmd.Parameters.AddWithValue("@telefono",telefono);
        string errore = "operazione effettuata!";
        try
        {
            cmd.ExecuteNonQuery();
        }
        catch(Exception e)
        {
            errore = e.Message;
        }
        return errore;
    }
    
    public string EliminaDettagliPrenotazione(string idPrenotazione,string idCamera)
    {
        string query = "DELETE FROM `hotel`.`dettagli_prenotazioni` WHERE  `idPrenotazione`=@idPrenotazione AND `idCamera`=@idCamera;"; 
        MySqlCommand cmd = new MySqlCommand(query,con);
        cmd.Parameters.AddWithValue("@idPrenotazione", idPrenotazione);
        cmd.Parameters.AddWithValue("@idCamera", idCamera);
        string errore = "ok";
        try
        {
            cmd.ExecuteNonQuery();
        }
        catch(Exception e)
        {
            errore = e.Message;
        }
        return errore;
    }
    public string EliminaPrenotazione(string id)
    {
        string query = "DELETE FROM `hotel`.`prenotazioni` WHERE id=@id"; 
        MySqlCommand cmd = new MySqlCommand(query,con);
        cmd.Parameters.AddWithValue("@id", id);

        string errore = "ok";
        try
        {
            cmd.ExecuteNonQuery();
        }
        catch(Exception e)
        {
            errore = e.Message;
        }
        return errore;
    }
}