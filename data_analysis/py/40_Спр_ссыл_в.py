#!/usr/bin/env python
# coding: utf-8

# <h1>Виды ссылочных документов (+)</h1>

# In[12]:


import pandas as pd
import numpy as np
import pyodbc 

db_conn = pyodbc.connect('Driver={SQL Server};'
                      'Server=LAPTOP-JSR6TV0G;'
                      'Database=Pro_t_londonSQL;'
                      'Trusted_Connection=yes;')


# In[13]:


df = pd.read_sql_query('SELECT * FROM Pro_t_londonSQL.dbo.Спр_ссыл_в', db_conn)
df.shape


# In[14]:


df


# In[15]:


df["Наим_вида"] = df["Наим_вида"].str.strip()
df["Наим_вида"] = df["Наим_вида"].str.capitalize()


# In[16]:


df = df.drop_duplicates(subset=["Наим_вида"])
df


# <h2>Rename columns</h2>

# In[17]:


df = df.rename(columns={"Вид_сд": "id",
                        "Наим_вида": "name"
                       })
df


# <h1>Postgres</h1>

# In[18]:


from psycopg2 import connect, sql, DatabaseError
import psycopg2.extras as extras


# In[19]:


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


# In[20]:


cursor.execute(open("sql/40.sql", "r").read())
conn.commit()


# <h2>Insert data</h2>

# In[21]:


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


# In[22]:


execute_values(conn, df, "linked_doc_types")


# In[ ]:




