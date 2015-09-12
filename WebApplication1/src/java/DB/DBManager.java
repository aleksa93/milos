/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package DB;

import javax.faces.bean.ManagedBean;
import javax.faces.bean.RequestScoped;
import java.io.Serializable;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.text.DecimalFormat;
import java.util.ArrayList;
import java.util.regex.Pattern;

import javax.faces.context.FacesContext;

/**
 *
 * @author solim
 */
@ManagedBean
@RequestScoped
public class DBManager implements Serializable{
    private String db_server   = "";
    private String db_user     = "";
    private String db_password = "";
    private String db_driver   = "";

    public Connection connection = null;
    /**
     * Creates a new instance of DBManager
     */
    public DBManager() throws Exception{
        init();
    }
    
    private void init()throws Exception{
        FacesContext fc = FacesContext.getCurrentInstance();
        db_server   = fc.getExternalContext().getInitParameter("DB-SERVER");
        db_user     = fc.getExternalContext().getInitParameter("DB-USER");
        db_password = fc.getExternalContext().getInitParameter("DB-PASSWORD");
        db_driver   = fc.getExternalContext().getInitParameter("JDBC-DRIVER");
        Class.forName(db_driver);
    } 
    
    public Connection initConnection() throws Exception{
        if( this.connection == null ){
            this.connection = DriverManager.getConnection(db_server, db_user, db_password);
            this.connection.setAutoCommit(false);
        }else if( this.connection.isClosed() ){
            this.connection = null;
            this.connection = DriverManager.getConnection(db_server, db_user, db_password);
            this.connection.setAutoCommit(false);
        }
        return this.connection;
    }

    public void closeConnection(){
        try {
            if( this.connection != null ){
                this.connection.close();
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    public void commitConnection(){
        try {
            if( this.connection != null && !this.connection.isClosed() ){
                this.connection.commit();
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    public void rollbackConnection(){
        try {
            if( this.connection != null && !this.connection.isClosed() ){
                this.connection.rollback();
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }
}
