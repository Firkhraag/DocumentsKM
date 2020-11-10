#!/usr/bin/env python
# coding: utf-8

# <h1>Прилагаемые документы марки</h1>

# In[9]:


import pandas as pd
import numpy as np
import pyodbc 

db_conn = pyodbc.connect('Driver={SQL Server};'
                      'Server=LAPTOP-JSR6TV0G;'
                      'Database=Pro_t_londonSQL;'
                      'Trusted_Connection=yes;')


# In[10]:


df = pd.read_sql_query('SELECT * FROM Pro_t_londonSQL.dbo.Од_прил', db_conn)
df.shape


# In[11]:


df.head()


# In[12]:


df = df.replace("", np.nan)


# In[13]:


df.isna().sum()


# In[14]:


df["код_марки"].min()


# In[15]:


df["наим_прил"].value_counts()


# In[16]:


df["обозн_прил"].value_counts()


# <h2>Дубликаты</h2>

# In[17]:


df[df.duplicated(subset=["код_марки", "обозн_прил"], keep=False)]


# In[18]:


df = df.drop_duplicates(subset=["код_марки", "обозн_прил"])
df.shape


# <h2>Null для бд</h2>

# In[19]:


df = df.where(pd.notnull(df), None)


# In[20]:


df.head()


# <h2>Добавляем id</h2>

# In[21]:


df.insert(0, 'id', range(1, len(df) + 1))


# <h2>Меняем названия столбцов</h2>

# In[22]:


df = df.rename(columns={"код_марки": "mark_id",
                        "наим_прил": "name",
                        "прим": "note",
                        "обозн_прил": "designation"
                       })
df


# In[23]:


m_ids = pd.read_csv("mark_ids.csv")
m_ids = m_ids.drop(["Unnamed: 0"], axis=1)
m_ids = m_ids.values.flatten().tolist()
df = df[df["mark_id"].isin(m_ids)]
df.shape


# <h1>Postgres</h1>

# <h2>Создание таблицы</h2>

# In[24]:


from psycopg2 import connect, sql, DatabaseError
import psycopg2.extras as extras


# DocumentsKM

# In[25]:


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


# In[26]:


cursor.execute(open("sql/12.sql", "r").read())
conn.commit()


# <h2>Вставка данных</h2>

# In[27]:


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


# In[28]:


execute_values(conn, df, "attached_docs")


# In[ ]:




