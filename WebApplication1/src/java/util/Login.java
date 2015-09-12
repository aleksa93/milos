/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package util;

import DB.DBManager;
import DB.Demonstrator;
import DB.Nastavnik;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.faces.bean.ManagedBean;
import javax.faces.bean.RequestScoped;

/**
 *
 * @author solim
 */
@ManagedBean
@RequestScoped
public class Login {
    public String username, password;
    private Demonstrator demonstrator;
    /**
     * Creates a new instance of Login
     */
    public Login() {
    }
    
    public void UlogujSe() throws SQLException, Exception{
        String query="SELECT ime,prezime FROM demonstrator WHERE username='"+this.username+"' AND password='"+this.password+"'";
        DBManager dbm = null;
        try {dbm = new DBManager();} catch (Exception ex) {Logger.getLogger(Login.class.getName()).log(Level.SEVERE, null, ex);}
        PreparedStatement preState = null;
        ResultSet resultSet        = null;
        Nastavnik n=null;
        preState   = dbm.initConnection().prepareStatement(query); 
        resultSet  = preState.executeQuery();
            while (resultSet.next()) {
                n=new Nastavnik(resultSet.getString(0),resultSet.getString(1));                
            }
    }

    public String getUsername() {
        return username;
    }

    public String getPassword() {
        return password;
    }

    public Demonstrator getDemonstrator() {
        return demonstrator;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public void setDemonstrator(Demonstrator demonstrator) {
        this.demonstrator = demonstrator;
    }
    
    
}
