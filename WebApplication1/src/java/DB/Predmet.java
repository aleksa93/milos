/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package DB;

import java.io.Serializable;

/**
 *
 * @author solim
 */
public class Predmet implements Serializable{
    private int id;
    private String naziv;    
    private String semestar;
    private String godina;    

    public Predmet(int id, String naziv, String semestar, String godina) {
        this.id = id;
        this.naziv = naziv;
        this.semestar = semestar;
        this.godina = godina;
    }

    public void setId(int id) {
        this.id = id;
    }

    public void setNaziv(String naziv) {
        this.naziv = naziv;
    }

    public void setSemestar(String semestar) {
        this.semestar = semestar;
    }

    public void setGodina(String godina) {
        this.godina = godina;
    }

    public int getId() {
        return id;
    }

    public String getNaziv() {
        return naziv;
    }

    public String getSemestar() {
        return semestar;
    }

    public String getGodina() {
        return godina;
    }
}
