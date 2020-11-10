#!/usr/bin/env python
# coding: utf-8

# <h1>Листы</h1>

# In[83]:


import pandas as pd
import numpy as np
import pyodbc 

db_conn = pyodbc.connect('Driver={SQL Server};'
                      'Server=LAPTOP-JSR6TV0G;'
                      'Database=Pro_t_londonSQL;'
                      'Trusted_Connection=yes;')


# In[84]:


df = pd.read_sql_query('SELECT * FROM Pro_t_londonSQL.dbo.Листы', db_conn)
df.shape


# In[85]:


df.head()


# In[86]:


df = df.replace("", np.nan)


# In[87]:


df.isna().sum()


# <h2>Номер отсутствует</h2>

# In[88]:


df[df["номер"].isna()]


# <h2>Удаляем выбросы</h2>

# In[89]:


df = df[df["номер"].isna() == False]


# <h2>Тип док отсутствует</h2>

# In[90]:


df[df["тип_док"].isna()]


# <h2>Удаляем</h2>

# In[91]:


df = df[df["тип_док"].isna() == False]


# In[92]:


df["тип_док"].value_counts()


# In[93]:


df["тип_док"] = df["тип_док"] + 1
df["тип_док"].value_counts()


# <h2>Название отсутствует</h2>

# In[94]:


df[df["название"].isna()]


# <h2>Вып, пров, норм = 0. Это FK, соответственно, он не может быть 0. Заменяем на null</h2>

# In[95]:


df["вып"] = df["вып"].map(lambda x: np.nan if x == 0 else x)


# In[96]:


df["пров"] = df["пров"].map(lambda x: np.nan if x == 0 else x)


# In[97]:


df["норм"] = df["норм"].map(lambda x: np.nan if x == 0 else x)


# <h2>Убираем шифры</h2>

# In[98]:


df = df.drop(["шифр_вып", "шифр_пров", "шифр_норм"], axis=1)
df.head()


# <h2>Заменяем null формат на default value = 1.0</h2>

# In[99]:


df["формат"] = df["формат"].map(lambda x: 1.0 if np.isnan(x) else x)


# In[100]:


df.isna().sum()


# In[101]:


df.shape


# <h2>Удаляем дубликаты</h2>

# In[102]:


df = df.drop_duplicates(subset=["код_марки", "номер", "тип_док"])
df.shape


# <h2>Исследуем данные</h2>

# In[103]:


df["формат"].value_counts()


# <h2>Формат = 125. Аномалия?</h2>

# In[104]:


df[df["формат"] == 125]


# In[105]:


df["вып"].value_counts()


# In[106]:


df["листов"].value_counts()


# In[107]:


df[df["прим"].isna() == False]


# <h2>Избавляемся от лишних пробелов</h2>

# In[108]:


df["название"] = df["название"].str.strip()


# In[109]:


for i in range(0, len(df)):
    try:
        arr = df["название"][i].split()
        new_str = ' '.join(arr)
        df.at[i, "название"] = new_str
    except Exception:
        pass


# In[110]:


df.head()


# <h2>Добавляем id</h2>

# In[111]:


df.insert(0, 'id', range(1, len(df) + 1))


# <h2>Меняем названия столбцов</h2>

# In[112]:


df = df.rename(columns={"код_марки": "mark_id",
                        "номер": "num",
                        "название": "name",
                        "формат": "form",
                        "вып": "creator_id",
                        "пров": "inspector_id",
                        "норм": "norm_contr_id",
                        "тип_док": "type_id",
                        "выпуск": "release_num",
                        "листов": "num_of_pages",
                        "прим": "note"
                       })
df


# <h2>Null для бд</h2>

# In[113]:


df = df.where(pd.notnull(df), None)


# In[114]:


df.head()


# In[115]:


m_ids = pd.read_csv("mark_ids.csv")
m_ids = m_ids.drop(["Unnamed: 0"], axis=1)
m_ids = m_ids.values.flatten().tolist()


# In[116]:


e_ids = pd.read_csv("employee_ids.csv")
e_ids = e_ids.drop(["Unnamed: 0"], axis=1)
e_ids = e_ids.values.flatten().tolist()


# In[117]:


df = df[df["mark_id"].isin(m_ids)]
df.loc[df["creator_id"].isin(e_ids) == False, "creator_id"] = None
df.loc[df["inspector_id"].isin(e_ids) == False, "inspector_id"] = None
df.loc[df["norm_contr_id"].isin(e_ids) == False, "norm_contr_id"] = None
df


# <h1>Postgres</h1>

# <h2>Создание таблицы</h2>

# In[118]:


from psycopg2 import connect, sql, DatabaseError
import psycopg2.extras as extras


# DocumentsKM

# In[119]:


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


# In[120]:


cursor.execute(open("sql/6.sql", "r").read())
conn.commit()


# <h2>Вставка данных</h2>

# In[121]:


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


# In[122]:


execute_values(conn, df, "docs")


# In[123]:


db_conn.close()


# In[ ]:




