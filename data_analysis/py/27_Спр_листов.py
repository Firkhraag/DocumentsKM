#!/usr/bin/env python
# coding: utf-8

# <h1>Справочник листов (типовые наименования листов) (+)</h1>

# In[1]:


import pandas as pd
import numpy as np
import pyodbc 

db_conn = pyodbc.connect('Driver={SQL Server};'
                      'Server=LAPTOP-JSR6TV0G;'
                      'Database=Pro_t_londonSQL;'
                      'Trusted_Connection=yes;')


# In[2]:


df = pd.read_sql_query('SELECT * FROM Pro_t_londonSQL.dbo.Спр_листов_н', db_conn)
df.shape


# In[3]:


df


# In[4]:


df["ш_листа"] = df["ш_листа"].str.strip()


# In[5]:


df.isna().sum()


# <h2>Меняем названия столбцов</h2>

# In[6]:


df = df.rename(columns={"ш_номер": "id",
                        "ш_листа": "name"
                       })
df


# <h1>Postgres</h1>

# <h2>Создание таблицы</h2>

# In[7]:


from psycopg2 import connect, sql, DatabaseError
import psycopg2.extras as extras


# DocumentsKM

# In[8]:


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


# In[9]:


cursor.execute(open("sql/27.sql", "r").read())
conn.commit()


# <h2>Вставка данных</h2>

# In[10]:


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


# In[11]:


execute_values(conn, df, "sheet_names")


# In[ ]:




