#!/usr/bin/env python
# coding: utf-8

# <h1>Исполнители</h1>

# In[37]:


import pandas as pd
import numpy as np
import pyodbc 

db_conn = pyodbc.connect('Driver={SQL Server};'
                      'Server=LAPTOP-JSR6TV0G;'
                      'Database=Pro_t_londonSQL;'
                      'Trusted_Connection=yes;')


# In[38]:


df = pd.read_sql_query('SELECT * FROM Pro_t_londonSQL.dbo.Спр_исполн', db_conn)
df.shape


# In[39]:


df


# In[40]:


df.isna().sum()


# In[41]:


df = df.dropna()


# In[42]:


df["код_отд"].value_counts()


# In[43]:


df.loc[:,"код_отд"] = df.loc[:,"код_отд"].replace(40, 1)
df.loc[:,"код_отд"] = df.loc[:,"код_отд"].replace(37, 1)
df.loc[:,"код_отд"] = df.loc[:,"код_отд"].replace(36, 1)
df.loc[:,"код_отд"] = df.loc[:,"код_отд"].replace(35, 26)
df.loc[:,"код_отд"] = df.loc[:,"код_отд"].replace(34, 1)
df.loc[:,"код_отд"] = df.loc[:,"код_отд"].replace(33, 26)
df.loc[:,"код_отд"] = df.loc[:,"код_отд"].replace(32, 1)
df.loc[:,"код_отд"] = df.loc[:,"код_отд"].replace(31, 26)
df.loc[:,"код_отд"] = df.loc[:,"код_отд"].replace(18, 9)
df.loc[:,"код_отд"] = df.loc[:,"код_отд"].replace(27, 26)
df.loc[:,"код_отд"] = df.loc[:,"код_отд"].replace(29, 3)


# In[44]:


df["код_отд"].value_counts()


# In[45]:


df = df[df["код_отд"] != 0]
df


# In[46]:


df["должность"] = df["должность"].str.strip()
df["должность"].value_counts()


# In[47]:


df.loc[df["должность"] == "ген.дир", 'тип_должн'] = 1
df.loc[df["должность"] == "гл.инж", 'тип_должн'] = 2
df.loc[df["должность"] == "зам.гл.инж", 'тип_должн'] = 3
df.loc[df["должность"] == "гл.стр", 'тип_должн'] = 4
df.loc[df["должность"] == "гл.арх", 'тип_должн'] = 5
df.loc[df["должность"] == "ГИП", 'тип_должн'] = 6
df.loc[df["должность"] == "нач.отд", 'тип_должн'] = 7
df.loc[df["должность"] == "зам.нач.отд", 'тип_должн'] = 8
df.loc[df["должность"] == "гл.спец.", 'тип_должн'] = 9
df.loc[df["должность"] == "Гл. спец.", 'тип_должн'] = 9
df.loc[df["должность"] == "зав.гр", 'тип_должн'] = 10
df.loc[df["должность"] == "Зав.гр", 'тип_должн'] = 10
df.loc[df["должность"] == "зав. Гр", 'тип_должн'] = 10
df.loc[df["должность"] == "вед.инж", 'тип_должн'] = 11
df.loc[df["должность"] == "вед.инж.", 'тип_должн'] = 11
df.loc[df["должность"] == "инж. 1 к", 'тип_должн'] = 12
df.loc[df["должность"] == "Инж. 1к", 'тип_должн'] = 12
df.loc[df["должность"] == "инженер1к", 'тип_должн'] = 12
df.loc[df["должность"] == "инж. 2 к", 'тип_должн'] = 13
df.loc[df["должность"] == "инж. 3 к", 'тип_должн'] = 14
df.loc[df["должность"] == "Инж. 3 к", 'тип_должн'] = 14
df.loc[df["должность"] == "инж3кат", 'тип_должн'] = 14
df.loc[df["должность"] == "инженер", 'тип_должн'] = 15
df.loc[df["должность"] == "инж", 'тип_должн'] = 15
df.loc[df["должность"] == "техник", 'тип_должн'] = 16
df.loc[df["должность"] == "чертежн", 'тип_должн'] = 17


# In[48]:


df["тип_должн"].value_counts()


# In[49]:


df["фио"] = df["фио"].str.strip()


# In[50]:


df = df.drop_duplicates(subset=["тип_должн", "фио", "код_отд"])


# <h2>Дропаем остальное, т. к. все равно данные не актуальны</h2>

# In[51]:


df = df[df["тип_должн"] != 0]


# In[52]:


df = df.drop(["шифр", "должность"], axis=1)
df.head()


# <h2>Rename columns</h2>

# In[53]:


df = df.rename(columns={"код": "id",
                        "тип_должн": "position_id",
                        "фио": "name",
                        "код_отд": "department_id"
                       })
df.head()


# <h1>Postgres</h1>

# In[54]:


from psycopg2 import connect, sql, DatabaseError
import psycopg2.extras as extras


# DocumentsKM

# In[55]:


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


# In[56]:


cursor.execute(open("sql/26.sql", "r").read())
conn.commit()


# <h2>Insert data</h2>

# In[57]:


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


# In[58]:


execute_values(conn, df, "employees")


# <h1>Employee ids to csv</h1>

# In[59]:


df["id"].to_csv("employee_ids.csv")


# In[ ]:




