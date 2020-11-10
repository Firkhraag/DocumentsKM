#!/usr/bin/env python
# coding: utf-8

# <h1>Варианты антикоррозионной защиты (+)</h1>

# In[61]:


import pandas as pd
import numpy as np
import pyodbc 

db_conn = pyodbc.connect('Driver={SQL Server};'
                      'Server=LAPTOP-JSR6TV0G;'
                      'Database=Pro_t_londonSQL;'
                      'Trusted_Connection=yes;')


# In[62]:


df = pd.read_sql_query('SELECT * FROM Pro_t_londonSQL.dbo.Варианты_аз', db_conn)
df.shape


# In[63]:


df.head()


# In[64]:


df = df.replace("", np.nan)


# In[65]:


df.isna().sum()


# In[66]:


df = df.dropna()


# <h2>Удаляем дубликаты</h2>

# In[67]:


df = df.drop_duplicates()
df.shape


# <h2>Исследуем данные</h2>

# In[68]:


df["Зона_экс"].value_counts()


# In[69]:


df["Группа_газов"].value_counts()


# In[70]:


df["Агресс_ср"].value_counts()


# In[71]:


df["Матер_к"].value_counts()


# In[72]:


df["Тип_лп"].value_counts()


# In[73]:


df["Группа_лп"].value_counts()


# In[74]:


df["Стойкость_лп"].value_counts()


# In[75]:


df["Кол_слоев_лп"].value_counts()


# In[76]:


df["Толщ_лпгр"].value_counts()


# In[77]:


df["Кол_слоев_гр"].value_counts()


# In[78]:


df["Степень_оч"].value_counts()


# In[79]:


df["Статус_аз"].value_counts()


# In[80]:


df = df.drop(["Статус_аз"], axis=1)
df.head()


# <h2>Добавляем id</h2>

# In[84]:


df.insert(0, 'id', range(1, len(df) + 1))


# In[85]:


df.head()


# In[86]:


df.shape


# In[87]:


df = df.drop_duplicates(subset=["Зона_экс", "Группа_газов", "Агресс_ср", "Матер_к", "Тип_лп", "Группа_лп", "Стойкость_лп"])
df.shape


# In[ ]:





# In[88]:


df[(df["Зона_экс"] == 2) & (df["Группа_газов"] == 1) & (df["Агресс_ср"] == 1) & (df["Матер_к"] == 0) & (df["Тип_лп"] == "ПФ")]


# In[89]:


df[(df["Зона_экс"] == 2) & (df["Группа_газов"] == 1) & (df["Агресс_ср"] == 2) & (df["Матер_к"] == 0) & (df["Тип_лп"] == "ПФ")]


# In[ ]:





# In[ ]:





# In[ ]:





# <h2>Меняем названия столбцов</h2>

# In[54]:


df = df.rename(columns={"Зона_экс": "operating_zone",
                        "Группа_газов": "gas_group",
                        "Агресс_ср": "env_aggressiveness",
                        "Матер_к": "material",
                        "Тип_лп": "paintwork_type",
                        "Группа_лп": "paintwork_group",
                        "Стойкость_лп": "paintwork_durability",
                        "Кол_слоев_лп": "paintwork_num_of_layers",
                        "Толщ_лпгр": "paintwork_primer_thickness",
                        "Кол_слоев_гр": "primer_num_of_layers",
                        "Степень_оч": "cleaning_degree_id"
                       })
df


# <h1>Postgres</h1>

# <h2>Создание таблицы</h2>

# In[55]:


from psycopg2 import connect, sql, DatabaseError
import psycopg2.extras as extras


# DocumentsKM

# In[56]:


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


# In[57]:


cursor.execute(open("sql/1.sql", "r").read())
conn.commit()


# <h2>Вставка данных</h2>

# In[58]:


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


# In[59]:


execute_values(conn, df, "corr_prot_variants")


# In[60]:


db_conn.close()


# In[ ]:




