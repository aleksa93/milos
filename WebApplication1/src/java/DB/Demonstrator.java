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
public class Demonstrator implements Serializable{
    private int id;
    private String username;    
    private String password;
    private String ime;    
    private String prezime;
    private String telefon;    
    private String email;
    private String ekstenzija;
    private String odsek;    
    private String godina;
    private Double prosek;

    public Demonstrator(int id, String username, String password, String ime, String prezime, String telefon, String email, String ekstenzija, String odsek, String godina, Double prosek) {
        this.id = id;
        this.username = username;
        this.password = password;
        this.ime = ime;
        this.prezime = prezime;
        this.telefon = telefon;
        this.email = email;
        this.ekstenzija = ekstenzija;
        this.odsek = odsek;
        this.godina = godina;
        this.prosek = prosek;
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

    public void setOdsek(String odsek) {
        this.odsek = odsek;
    }

    public void setGodina(String godina) {
        this.godina = godina;
    }

    public void setProsek(Double prosek) {
        this.prosek = prosek;
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

    public String getOdsek() {
        return odsek;
    }

    public String getGodina() {
        return godina;
    }

    public Double getProsek() {
        return prosek;
    }
    
}
