#!/usr/bin/env python
# coding: utf-8

# <h1>Типы документов (+)</h1>

# In[1]:


import pandas as pd
import numpy as np


# In[2]:


df = pd.DataFrame({'id': [1, 2, 3, 4, 5], 'code': ["л.", "СМ", "ВМП", "СМС", "РР"], 'name': ["Листы основного комплекта", "Спецификация металлопроката", "Ведомость металлоконструкций по видам профилей", "Сводная спецификация металлопроката", "Расчет металлоконструкций"]})
df


# <h1>Postgres</h1>

# In[3]:


from psycopg2 import connect, sql, DatabaseError
import psycopg2.extras as extras


# DocumentsKM

# In[4]:


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


# In[5]:


cursor.execute(open("sql/x.sql", "r").read())
conn.commit()


# <h2>Вставка данных</h2>

# In[6]:


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


# In[7]:


execute_values(conn, df, "doc_types")


# In[ ]:




