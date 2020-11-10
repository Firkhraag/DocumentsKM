#!/usr/bin/env python
# coding: utf-8

# <h1>Обозначения марок (+)</h1>

# In[15]:


import pandas as pd
import numpy as np
import pyodbc 

db_conn = pyodbc.connect('Driver={SQL Server};'
                      'Server=LAPTOP-JSR6TV0G;'
                      'Database=Pro_t_londonSQL;'
                      'Trusted_Connection=yes;')


# In[16]:


df = pd.read_sql_query('SELECT * FROM Pro_t_londonSQL.dbo.Об_марок', db_conn)
df.shape


# In[17]:


df


# <h2>Убираем первую и последнюю строку, добавляем КМД</h2>

# In[18]:


df = df.iloc[1:, :]
df = df.iloc[:-1, :]

# df2 = pd.DataFrame([[5, 6], [7, 8]], columns=list('AB'))
df.loc[len(df) + 1] = ["КМД", "Конструкции металлические деталировочные"]
df


# <h2>Добавляем id</h2>

# In[19]:


df.insert(0, 'id', range(1, len(df) + 1))


# <h2>Меняем названия столбцов</h2>

# In[20]:


df = df.rename(columns={"об_марки": "designation",
                        "наим_марки": "name"
                       })
df


# <h1>Postgres</h1>

# <h2>Создание таблицы</h2>

# In[21]:


from psycopg2 import connect, sql, DatabaseError
import psycopg2.extras as extras


# DocumentsKM

# In[22]:


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


# In[23]:


cursor.execute(open("sql/9.sql", "r").read())
conn.commit()


# <h2>Вставка данных</h2>

# In[24]:


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


# In[25]:


execute_values(conn, df, "mark_designations")


# In[ ]:




