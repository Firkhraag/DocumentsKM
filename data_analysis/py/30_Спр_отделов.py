#!/usr/bin/env python
# coding: utf-8

# <h1>Отделы</h1>

# In[4]:


import pandas as pd
import numpy as np
import pyodbc 

db_conn = pyodbc.connect('Driver={SQL Server};'
                      'Server=LAPTOP-JSR6TV0G;'
                      'Database=Pro_t_londonSQL;'
                      'Trusted_Connection=yes;')


# In[5]:


df = pd.read_sql_query('SELECT * FROM Pro_t_londonSQL.dbo.Спр_отделов', db_conn)
df.shape


# In[6]:


df


# In[7]:


df = df.drop(["начальник", "шифр_отд"], axis=1)
df


# In[8]:


df["наименование"] = df["наименование"].str.strip()


# In[9]:


df


# In[10]:


df = df[df["код_отд"] != 40]
df = df[df["код_отд"] != 37]
df = df[df["код_отд"] != 36]
df = df[df["код_отд"] != 35]
df = df[df["код_отд"] != 34]
df = df[df["код_отд"] != 33]
df = df[df["код_отд"] != 32]
df = df[df["код_отд"] != 31]
df = df[df["код_отд"] != 18]
df = df[df["код_отд"] != 27]
df


# In[11]:


df.at[0, "наименование"] = "СО-2б"
df


# In[12]:


df.at[1, "наименование"] = "Машиностроительный"
df


# In[13]:


df = df[df["код_отд"] != 29]
df


# In[14]:


df.at[30, "наименование"] = "СО-2а"
df


# In[15]:


df = df.rename(columns={"код_отд": "id",
                        "наименование": "name"
                       })
df.head()


# In[16]:


df["name"].value_counts()


# <h1>Postgres</h1>

# In[17]:


from psycopg2 import connect, sql, DatabaseError
import psycopg2.extras as extras


# DocumentsKM

# In[18]:


# Connect
try:
    conn = connect (
        dbname = "documentskm",
        user = "postgres",
        host = "localhost",
        password = "password"
    )
    cursor = conn.cursor()
except Exception as err:
    cursor = None
    print("Psycopg2 error:", err)
    
# Check if the connection was valid
if cursor != None:
    print("Connection successful")


# In[19]:


cursor.execute(open("sql/30.sql", "r").read())
conn.commit()


# <h2>Data</h2>

# In[20]:


def execute_values(conn, df, table):
    tuples = [tuple(x) for x in df.to_numpy()]
    cols = ','.join(list(df.columns))
    query  = "INSERT INTO %s(%s) VALUES %%s" % (table, cols)
    cursor = conn.cursor()
    try:
        extras.execute_values(cursor, query, tuples)
        conn.commit()
    except (Exception, DatabaseError) as error:
        print("Error: %s" % error)
        conn.rollback()
        cursor.close()
        return 1
    print("execute_values() done")
    cursor.close()


# In[21]:


execute_values(conn, df, "departments")


# In[ ]:




