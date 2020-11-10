#!/usr/bin/env python
# coding: utf-8

# <h1>Степени очистки аз (+)</h1>

# In[12]:


import pandas as pd
import numpy as np
import pyodbc 

db_conn = pyodbc.connect('Driver={SQL Server};'
                      'Server=LAPTOP-JSR6TV0G;'
                      'Database=Pro_t_londonSQL;'
                      'Trusted_Connection=yes;')


# In[13]:


df = pd.read_sql_query('SELECT * FROM Pro_t_londonSQL.dbo.Степени_оч_аз', db_conn)
df.shape


# In[14]:


df


# In[15]:


df["Наим_оч"] = df["Наим_оч"].str.strip()


# <h2>Rename columns</h2>

# In[16]:


df = df.rename(columns={"Степень_оч": "id",
                        "Наим_оч": "name"
                       })
df


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


cursor.execute(open("sql/44.sql", "r").read())
conn.commit()


# <h2>Insert data</h2>

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


execute_values(conn, df, "corr_prot_cleaning_degrees")


# In[ ]:




