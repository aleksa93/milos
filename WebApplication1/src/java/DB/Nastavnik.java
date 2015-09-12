/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package DB;

import java.io.Serializable;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

/**
 *
 * @author solim
 */
public class Nastavnik implements Serializable{
    private int id;
    private String username;    
    private String password;
    private String ime;    
    private String prezime;
    private String telefon;    
    private String email;
    private String ekstenzija;

    public Nastavnik(int id, String username, String password, String ime, String prezime, String telefon, String email, String ekstenzija) {
        super();
        this.id = id;
        this.username = username;
        this.password = password;
        this.ime = ime;
        this.prezime = prezime;
        this.telefon = telefon;
        this.email = email;
        this.ekstenzija = ekstenzija;
    }
    
    public Nastavnik(String ime,String prezime){
        this.ime=ime;
        this.prezime=prezime;
    }

    public void setId(int id) {
        this.id = id;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public void setIme(String ime) {
        this.ime = ime;
    }

    public void setPrezime(String prezime) {
        this.prezime = prezime;
    }

    public void setTelefon(String telefon) {
        this.telefon = telefon;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public void setEkstenzija(String ekstenzija) {
        this.ekstenzija = ekstenzija;
    }

    public int getId() {
        return id;
    }

    public String getUsername() {
        return username;
    }

    public String getPassword() {
        return password;
    }

    public String getIme() {
        return ime;
    }

    public String getPrezime() {
        return prezime;
    }

    public String getTelefon() {
        return telefon;
    }

    public String getEmail() {
        return email;
    }

    public String getEkstenzija() {
        return ekstenzija;
    }
    
    public static List<Nastavnik> getListByCategory(DBManager dbm, String cid, boolean close) throws Exception{
        List<Nastavnik> list = new ArrayList<Nastavnik>();
        PreparedStatement preState = null;
        ResultSet resultSet        = null;
        try {
            if( dbm == null ){
                dbm = new DBManager();
            }
            String sql = "SELECT * FROM nastavnik";
            preState   = dbm.initConnection().prepareStatement(sql); 
            preState.setString(1, cid);
            resultSet  = preState.executeQuery();
            while (resultSet.next()) {
                list.add( new Nastavnik(resultSet.getInt(0),resultSet.getString(1),resultSet.getString(2),resultSet.getString(3),resultSet.getString(4),resultSet.getString(5),resultSet.getString(6),resultSet.getString(7)) );                
            }
        }catch (Exception e) {
            e.printStackTrace();
        }finally{
            if( preState != null )
                preState.close();
            if( close && dbm.connection != null )
                dbm.connection.close();         
        }
        return list;
    }
}
