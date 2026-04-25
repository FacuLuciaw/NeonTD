using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PartidaJSON
{
    public int id_user; // Lo obtendrías al hacer login
    public string estado; // 'victoria', 'derrota', 'rendicion'
    public string nivel;  // 'facil', 'normal', 'dificil'
    public int dano_total_infligido;
    public int dano_total_recibido;
    public int oro_ganado;
    public int oro_gastado;
    
    // Listas para las tablas intermedias (N:M)
    public List<DetalleTorre> torres = new List<DetalleTorre>();
    public List<DetalleEnemigo> enemigos = new List<DetalleEnemigo>();

    // Método para registrar una torre construida
    public void RegistrarTorre(string nombreTorre)
    {
        // Buscamos si ya existe una torre con ese nombre
        DetalleTorre torreExistente = torres.Find(t => t.nombre == nombreTorre);
        
        if (torreExistente != null)
        {
            // Si existe, aumentamos la cantidad
            torreExistente.cantidad++;
        }
        else
        {
            // Si no existe, la creamos
            torres.Add(new DetalleTorre { nombre = nombreTorre, cantidad = 1 });
        }
    }

    // Método para registrar un enemigo derrotado
    public void RegistrarEnemigo(string nombreEnemigo)
    {
        // Buscamos si ya existe un enemigo con ese nombre
        DetalleEnemigo enemigoExistente = enemigos.Find(e => e.nombre == nombreEnemigo);
        
        if (enemigoExistente != null)
        {
            // Si existe, aumentamos la cantidad
            enemigoExistente.cantidad++;
        }
        else
        {
            // Si no existe, lo creamos
            enemigos.Add(new DetalleEnemigo { nombre = nombreEnemigo, cantidad = 1 });
        }
    }

    // Método para restar una torre (cuando se vende)
    public void DesvincularTorre(string nombreTorre)
    {
        // Buscamos la torre con ese nombre
        DetalleTorre torreExistente = torres.Find(t => t.nombre == nombreTorre);
        
        if (torreExistente != null)
        {
            // Si existe y hay más de 1, restamos 1
            if (torreExistente.cantidad > 1)
            {
                torreExistente.cantidad--;
            }
            else
            {
                // Si solo hay 1, la eliminamos de la lista
                torres.Remove(torreExistente);
            }
        }
    }

    // Métodos para rastrear dinero ganado y gastado
    public void RegistrarOroGanado(int cantidad)
    {
        oro_ganado += cantidad;
       
    }

    public void RegistrarOroGastado(int cantidad)
    {
        oro_gastado += cantidad;
       
    }

    // Método para restar oro gastado (cuando se vende una torre)
    public void RestarOroGastado(int cantidad)
    {
        oro_gastado -= cantidad;
        // Aseguramos que no sea negativo
        if (oro_gastado < 0) oro_gastado = 0;
    }

    // Método para registrar daño recibido por la base
    public void RegistrarDanoRecibido(float cantidad)
    {
        dano_total_recibido += (int)cantidad;
       
    }

    // Método para establecer el nivel de dificultad
    public void EstablecerNivel(string nombreNivel)
    {
        nivel = nombreNivel;
    }
}

[Serializable]
public class DetalleTorre {
    public string nombre;
    public int cantidad;
}

[Serializable]
public class DetalleEnemigo {
    public string nombre;
    public int cantidad;
}