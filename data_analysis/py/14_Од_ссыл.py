#!/usr/bin/env python
# coding: utf-8

# <h1>Ссылочные документы марки</h1>

# In[22]:


import pandas as pd
import numpy as np
import pyodbc 

db_conn = pyodbc.connect('Driver={SQL Server};'
                      'Server=LAPTOP-JSR6TV0G;'
                      'Database=Pro_t_londonSQL;'
                      'Trusted_Connection=yes;')


# In[23]:


df = pd.read_sql_query('SELECT * FROM Pro_t_londonSQL.dbo.Од_ссыл', db_conn)
df.shape


# In[24]:


df.head()


# In[25]:


df = df.replace("", np.nan)


# In[26]:


df.isna().sum()


# Все примечания = null

# Удаляем столбец на данный момент

# In[27]:


df = df.drop(["прим"], axis=1)
df.head()


# In[28]:


df["шифр_сд"].value_counts()


# 1? Аномалия?

# In[29]:


df[df["шифр_сд"] == "1"]


# In[30]:


df= df[df["шифр_сд"] != "1"]
df["шифр_сд"] = df["шифр_сд"].str.upper()
df["шифр_сд"].value_counts()


# In[31]:


m_ids = pd.read_csv("mark_ids.csv")
m_ids = m_ids.drop(["Unnamed: 0"], axis=1)
m_ids = m_ids.values.flatten().tolist()
df = df[df["код_марки"].isin(m_ids)]
df.shape


# Нет ссылочных документов для текущих марок

# <h2>Random generation of linked documents</h2>

# In[32]:


import random


# In[33]:


for i in range(1, 301):
    df = df.append(pd.Series(), ignore_index=True)


# In[34]:


df.loc[:, "код_марки"] = random.choices(m_ids, k=300)
df.loc[:, "шифр_сд"] = random.choices(range(1, 311), k=300)


# In[35]:


df = df.drop_duplicates(subset=["код_марки", "шифр_сд"])
df.head()


# <h2>Id</h2>

# In[36]:


df.insert(0, 'id', range(1, len(df) + 1))


# <h2>Меняем названия столбцов</h2>

# In[37]:


df = df.rename(columns={"код_марки": "mark_id",
                        "шифр_сд": "linked_doc_id",
                        "прим": "note"
                       })
df


# <h1>Postgres</h1>

# Создание таблицы

# In[38]:


from psycopg2 import connect, sql, DatabaseError
import psycopg2.extras as extras

from psycopg2.extensions import register_adapter, AsIs
register_adapter(np.int32, AsIs)
register_adapter(np.int64, AsIs)


# DocumentsKM

# In[39]:


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


# In[40]:


cursor.execute(open("sql/14.sql", "r").read())
conn.commit()


# <h2>Вставка данных</h2>

# In[41]:


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


# In[42]:


execute_values(conn, df, "mark_linked_docs")


# In[ ]:




