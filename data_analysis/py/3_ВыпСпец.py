#!/usr/bin/env python
# coding: utf-8

# <h1>Выпуски спецификации</h1>

# In[129]:


import pandas as pd
import numpy as np
import pyodbc 

db_conn = pyodbc.connect('Driver={SQL Server};'
                      'Server=LAPTOP-JSR6TV0G;'
                      'Database=Pro_t_londonSQL;'
                      'Trusted_Connection=yes;')


# In[130]:


df = pd.read_sql_query('SELECT * FROM Pro_t_londonSQL.dbo.ВыпСпец', db_conn)
df.shape


# In[131]:


df.head()


# In[132]:


df = df.replace("", np.nan)


# In[133]:


df.isna().sum()


# <h2>Имеется примечание</h2>

# In[134]:


df[df["прим"].isna() == False]


# <h2>Удаляем дубликаты</h2>

# In[135]:


df = df.drop_duplicates(subset=["код_марки", "выпуск"])
df.shape


# <h2>Удаляем вкл_вып (?)</h2>

# In[136]:


df = df.drop(["вкл_вып"], axis=1)
df.head()


# <h2>Исследуем данные</h2>

# In[137]:


df["выпуск"].value_counts()


# In[138]:


df["дата_созд"].max()


# In[139]:


df["дата_созд"].min()


# In[140]:


df["текущий"].value_counts()


# In[141]:


df["текущий"] = df["текущий"].map(lambda x: True if (x == 1) else False)


# <h2>Добавляем id</h2>

# In[142]:


df.insert(0, 'id', range(1, len(df) + 1))


# <h2>Меняем названия столбцов</h2>

# In[143]:


df = df.rename(columns={"код_марки": "mark_id",
                        "выпуск": "num",
                        "дата_созд": "created_date",
                        "текущий": "is_current",
                        "прим": "note"
                       })
df


# <h2>Null для бд</h2>

# In[144]:


df["created_date"] = df["created_date"].astype(object).where(df["created_date"].notnull(), None)
df = df.where(pd.notnull(df), None)


# In[145]:


df.head()


# In[146]:


m_ids = pd.read_csv("mark_ids.csv")
m_ids = m_ids.drop(["Unnamed: 0"], axis=1)
m_ids = m_ids.values.flatten().tolist()


# In[147]:


df = df[df["mark_id"].isin(m_ids)]
df


# <h1>Postgres</h1>

# <h2>Создание таблицы</h2>

# In[148]:


from psycopg2 import connect, sql, DatabaseError
import psycopg2.extras as extras


# DocumentsKM

# In[149]:


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


# In[150]:


cursor.execute(open("sql/3.sql", "r").read())
conn.commit()


# <h2>Вставка данных</h2>

# In[151]:


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


# In[152]:


execute_values(conn, df, "specifications")


# In[ ]:




