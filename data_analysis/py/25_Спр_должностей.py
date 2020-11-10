#!/usr/bin/env python
# coding: utf-8

# <h1>Должности</h1>

# In[2]:


import pandas as pd
import numpy as np
import pyodbc 

db_conn = pyodbc.connect('Driver={SQL Server};'
                      'Server=LAPTOP-JSR6TV0G;'
                      'Database=Pro_t_londonSQL;'
                      'Trusted_Connection=yes;')


# In[3]:


df = pd.read_sql_query('SELECT * FROM Pro_t_londonSQL.dbo.Спр_должностей', db_conn)
df.shape


# In[4]:


df


# In[5]:


df.at[13, "наим_должн"] = "инж. 3 к"


# In[6]:


df


# <h2>Меняем названия столбцов</h2>

# In[7]:


df = df.rename(columns={"должность": "id",
                        "наим_должн": "name"
                       })
df.head()


# <h1>Postgres</h1>

# <h2>Создание таблицы</h2>

# In[8]:


from psycopg2 import connect, sql, DatabaseError
import psycopg2.extras as extras


# DocumentsKM

# In[9]:


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


# In[10]:


cursor.execute(open("sql/25.sql", "r").read())
conn.commit()


# <h2>Вставка данных</h2>

# In[11]:


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


# In[12]:


execute_values(conn, df, "positions")


# In[ ]:




