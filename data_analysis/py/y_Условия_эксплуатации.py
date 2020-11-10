#!/usr/bin/env python
# coding: utf-8

# In[3]:


import pandas as pd
import numpy as np


# In[5]:


df1 = pd.DataFrame({'id': [1, 2, 3, 4], 'name': ["неагрессивная", "слабоагрессивная", "среднеагрессивная", "сильноагрессивная"]})
df1


# In[6]:


df2 = pd.DataFrame({'id': [1, 2, 3], 'name': ["внутри помещения", "снаружи помещения", "в жидкостях"]})
df2


# In[7]:


df3 = pd.DataFrame({'id': [1, 2, 3, 4, 5], 'name': ["нет газов (вода)", "газы группы А (кислоты)", "газы группы B, C и D (щелочи)", "бензин", "масло"]})
df3


# In[8]:


df4 = pd.DataFrame({'id': [1, 2, 3, 4], 'name': ["сталь без покрытия", "оцинкованная сталь", "сталь горячего цинкования", "сталь с газотермическим напылением цинка"]})
df4


# In[16]:


# df5 = pd.DataFrame({'id': [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16], 'code': ["ПФ", "АУ", "ЭЭ", "МА", "МБ", "НЦ", "ФФ", "ПА", "ПЭ", "ПБ", "ПВ", "СВ", "ПУ", "ЭП", "ПС", "КО"], 'name': ["пентафталевый", "алкидно-уретановый", "эпоксиэфирный", "масляный", "маслянобитумный", "нитроцеллюлозный", "ФФ", "ПА", "полиэфирный ненасыщенный", "ПБ", "ПВ", "СВ", "ПУ", "эпоксидный", "полистирольный", "кремнийорганический"]})
df5 = pd.DataFrame({'id': [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16], 'name': ["ПФ", "АУ", "ЭЭ", "МА", "МБ", "НЦ", "ФФ", "ПА", "ПЭ", "ПБ", "ПВ", "СВ", "ПУ", "ЭП", "ПС", "КО"]})
df5


# In[10]:


df6 = pd.DataFrame({'id': [1, 2, 3, 4], 'name': ["отсутствуют", "сдвигоустойчивые соединения", "фланцевые соединения", "сдвигоустойчивые и фланцевые соединения"]})
df6


# <h1>Postgres</h1>

# In[11]:


from psycopg2 import connect, sql, DatabaseError
import psycopg2.extras as extras


# DocumentsKM

# In[12]:


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


# In[17]:


cursor.execute(open("sql/y_1.sql", "r").read())
cursor.execute(open("sql/y_2.sql", "r").read())
cursor.execute(open("sql/y_3.sql", "r").read())
cursor.execute(open("sql/y_4.sql", "r").read())
cursor.execute(open("sql/y_5.sql", "r").read())
cursor.execute(open("sql/y_6.sql", "r").read())
conn.commit()


# <h2>Вставка данных</h2>

# In[14]:


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


# In[18]:


execute_values(conn, df1, "env_aggressiveness")
execute_values(conn, df2, "operating_areas")
execute_values(conn, df3, "gas_groups")
execute_values(conn, df4, "construction_materials")
execute_values(conn, df5, "paintwork_types")
execute_values(conn, df6, "high_tensile_bolts_types")


# In[ ]:




