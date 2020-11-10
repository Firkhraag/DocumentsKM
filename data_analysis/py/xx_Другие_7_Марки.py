#!/usr/bin/env python
# coding: utf-8

# <h1>Проекты, узлы, подузлы</h1>

# Грубые разбиения

# In[270]:


import pandas as pd
import numpy as np
import pyodbc 

db_conn = pyodbc.connect('Driver={SQL Server};'
                      'Server=LAPTOP-JSR6TV0G;'
                      'Database=Pro_t_londonSQL;'
                      'Trusted_Connection=yes;')


# In[271]:


df = pd.read_sql_query('SELECT * FROM Pro_t_2.dbo.Марки', db_conn)
df.shape


# In[272]:


df.head()


# In[273]:


df.tail()


# In[274]:


# df = df[["код_марки", "обозн", "комплекс", "объект"]]
# df


# In[275]:


df = df.dropna(subset=["код_марки", "обозн", "комплекс", "объект"])


# In[276]:


e_ids = pd.read_csv("employee_ids.csv")
e_ids = e_ids.drop(["Unnamed: 0"], axis=1)
e_ids = e_ids.values.flatten().tolist()
df.loc[df["гл_спец"].isin(e_ids) == False, "гл_спец"] = None
df.loc[df["рук_гр"].isin(e_ids) == False, "рук_гр"] = None
df.loc[df["н_контр"].isin(e_ids) == False, "н_контр"] = None


# In[277]:


df


# In[278]:


df = df[df["обозн"].str.contains("демонтаж") == False]
df = df[df["обозн"].str.contains("Мновый") == False]
df["обозн"] = df["обозн"].str.replace(",", ".")
df


# In[279]:


df2 = pd.DataFrame(df["обозн"].str.split('-КМ', 1).tolist(),
                                 columns = ['first','code'])
df2.isna().sum()


# In[280]:


df = df.reset_index()
df2 = df2.join(df)


# In[281]:


df2


# In[282]:


df2 = df2.dropna(subset=["code"])


# In[283]:


df2["code"] = "КМ" + df2["code"]


# In[284]:


df2


# In[285]:


df3 = pd.DataFrame(df2["first"].str.split('.', 1).tolist(),
                                 columns = ['baseSeries','middle'])
df3


# In[286]:


df2 = df2.reset_index()
df3 = df3.reset_index()
df4 = df2.join(df3[["baseSeries", "middle"]])
df4


# In[287]:


df4["baseSeries"].value_counts()


# In[288]:


df4 = df4.drop(["first", "index"], axis=1)
df4 = df4.rename(columns={"код_марки": "id"})
df4.head()


# In[289]:


df5 = pd.DataFrame(df4["middle"].fillna("").str.split('.', 1).tolist(),
                                 columns = ['node','subnode'])
df5


# In[290]:


df6 = pd.DataFrame(df5["subnode"].fillna("").str.split('-', 1).tolist(),
                                 columns = ['subnode_part','node_part'])
df6[df6["node_part"].isna() == False]


# In[291]:


df5 = df5.reset_index()
df6 = df6.reset_index()
df7 = df5.join(df6[["subnode_part", "node_part"]])
df7


# In[292]:


df7["node_part"] = "-" + df7["node_part"]
df7["node2"] = df7["node"] + df7["node_part"].fillna("")
df7


# In[293]:


df4 = df4.reset_index(drop=True)
df7 = df7.reset_index(drop=True)
df8 = df4.join(df7[["node2", "subnode_part"]])
df8


# In[294]:


df8 = df8.rename(columns={"node2": "nodeCode", "subnode_part": "subnodeCode"})
df8 = df8.drop(["middle"], axis=1)
df8.head()


# In[295]:


df8[df8["baseSeries"] == "М32833"]


# <h2>Enconding</h2>

# In[296]:


from sklearn.preprocessing import OrdinalEncoder


# In[297]:


enc = OrdinalEncoder()
res = enc.fit_transform(np.array(df8["baseSeries"]).reshape(-1, 1))
res


# In[298]:


enc.categories_


# In[299]:


s = pd.DataFrame(pd.Series(res.flatten()) + 1)
s = s.rename(columns={0: "project_id"})
s


# In[300]:


df9 = df8.join(s)
df9


# In[301]:


projects_df = df9[["baseSeries", "комплекс", "project_id"]]
projects_df


# In[302]:


projects_df = projects_df.drop_duplicates(subset=["project_id"])
projects_df


# In[303]:


projects_df = projects_df.rename(columns={"baseSeries": "base_series", "комплекс": "name", "project_id": "id"})
projects_df


# In[304]:


projects_df["name"].value_counts()


# In[305]:


projects_df["base_series"].value_counts()


# <h1>Postgres</h1>

# In[163]:


from psycopg2 import connect, sql, DatabaseError
import psycopg2.extras as extras


# DocumentsKM

# In[164]:


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


# In[238]:


cursor.execute(open("sql/xx_projects.sql", "r").read())
conn.commit()


# In[165]:


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


# In[240]:


execute_values(conn, projects_df, "projects")


# <h1>Nodes</h1>

# In[306]:


df10 = pd.DataFrame(df9["объект"].str.split('.', 1).tolist(),
                                 columns = ['f_p','o_p'])
df10


# In[307]:


df10[df10["o_p"].isna()]


# In[308]:


df10 = df10.fillna("")


# In[309]:


df11 = df9.join(df10)
df11


# In[310]:


df11["node_feature"] = df11["baseSeries"] + df11["nodeCode"]
df11


# In[311]:


enc = OrdinalEncoder()
res = enc.fit_transform(np.array(df11["node_feature"]).reshape(-1, 1))
res


# In[312]:


s = pd.DataFrame(pd.Series(res.flatten()) + 1)
s = s.rename(columns={0: "node_id"})
s


# In[313]:


df12 = df11.join(s)
df12


# In[314]:


nodes_df = df12[["node_id", "f_p", "project_id", "nodeCode"]]
nodes_df


# In[315]:


import random


# In[316]:


chiefEngineerIds = [92, 123, 144, 169, 201, 270, 305, 311, 368, 370, 396, 400, 443, 461, 462, 604, 608]


# In[317]:


nodes_df["chief_engineer"] = random.choices(chiefEngineerIds, k=len(nodes_df))
nodes_df


# In[318]:


nodes_df = nodes_df.rename(columns={"node_id": "id", "f_p": "name", "nodeCode": "code", "chief_engineer": "chief_engineer_id"})
nodes_df


# In[319]:


nodes_df = nodes_df.drop_duplicates(subset=["id"])


# <h1>Postgres</h1>

# In[255]:


cursor.execute(open("sql/xx_nodes.sql", "r").read())
conn.commit()


# In[256]:


execute_values(conn, nodes_df, "nodes")


# <h1>Subnodes</h1>

# In[320]:


df12


# In[321]:


df13 = pd.DataFrame(df12["o_p"].str.split('.', 1).tolist(),
                                 columns = ['s_p','m_p'])
df13


# In[322]:


df13 = df13.fillna("")


# In[323]:


df13["s_p"] = df13["s_p"].str.strip()
df13["m_p"] = df13["m_p"].str.strip()


# In[324]:


df14 = df12.join(df13)
df14


# In[325]:


df14["subnode_feature"] = df11["node_feature"] + df11["subnodeCode"]
df14


# In[326]:


enc = OrdinalEncoder()
res = enc.fit_transform(np.array(df14["subnode_feature"]).reshape(-1, 1))
res


# In[327]:


s = pd.DataFrame(pd.Series(res.flatten()) + 1)
s = s.rename(columns={0: "subnode_id"})
s


# In[328]:


df15 = df14.join(s)
df15


# In[329]:


subnodes_df = df15[["subnode_id", "node_id", "subnodeCode", "s_p"]]
subnodes_df


# In[330]:


subnodes_df = subnodes_df.rename(columns={"subnode_id": "id", "s_p": "name", "subnodeCode": "code"})
subnodes_df


# In[331]:


subnodes_df = subnodes_df.drop_duplicates(subset=["id"])


# <h1>Postgres</h1>

# In[269]:


cursor.execute(open("sql/xx_subnodes.sql", "r").read())
conn.commit()


# In[270]:


execute_values(conn, subnodes_df, "subnodes")


# <h1>Marks</h1>

# In[332]:


df15 = df15.drop(["обозн", "комплекс", "объект", "level_0", "ОЗ_марки"], axis=1)


# In[333]:


df15["t_эксплуат"] = df15["t_эксплуат"].str.replace("минус ", "-")
df15["t_эксплуат"] = df15["t_эксплуат"].str.replace("плюс ", "")
df15["t_эксплуат"] = df15["t_эксплуат"].str.replace("+", "")
df15["t_эксплуат"] = df15["t_эксплуат"].str.replace("- 39", "-39")
df15 = df15[df15["t_эксплуат"] != "-"]
df15 = df15[df15["t_эксплуат"] != "минус"]
df15["t_эксплуат"] = df15["t_эксплуат"].astype("float64")


# In[334]:


df15["t_эксплуат"].value_counts()


# In[335]:


df15["об_марки"] = df15["об_марки"].str.strip()
df15["об_марки"] = df15["об_марки"].str.replace("KM", "КМ")
df15 = df15[df15["об_марки"] != "по"]
df15["об_марки"].value_counts()


# In[336]:


df15 = df15.drop(["об_марки"], axis=1)
df15.head()


# In[337]:


df15 = df15.drop(["project_id", "f_p", "o_p", "s_p", "subnode_feature", "node_feature", "subnodeCode"], axis=1)
df15.head()


# In[338]:


df15 = df15.drop(["baseSeries", "nodeCode", "node_id"], axis=1)
df15.head()


# In[339]:


df15["тип_ЛКМ"].value_counts()


# In[340]:


df15["прим"].value_counts()


# In[341]:


df15["прим"] = df15["прим"].str.strip()
df15["прим"] = df15["прим"].str.replace("\r\n       ", " ")
df15["прим"].value_counts()


# In[342]:


df15["ДопКод"].value_counts()


# In[343]:


df15 = df15.drop(["ДопКод"], axis=1)
df15.head()


# In[344]:


df15["ВыпЗдСм"].value_counts()


# In[345]:


df15["10ХСНД"].value_counts()


# In[346]:


df15.isna().sum()


# In[347]:


df15["категория_пр"].value_counts()


# In[348]:


df15["категория_пр"] = 3


# In[349]:


# Unique values of subset
df15[["зона_эксплуат", "группа_газов", "агрессивность", "материал", "тип_ЛКМ"]].drop_duplicates()


# In[350]:


df15["агрессивность"] += 1
df15["группа_газов"] += 1
df15["материал"] += 1
df15["вп_болты"] += 1

# Part
df15.loc[df15["тип_ЛКМ"] == "ПФ", "тип_ЛКМ"] = 1
df15.loc[df15["тип_ЛКМ"] == "АУ", "тип_ЛКМ"] = 2
df15.loc[df15["тип_ЛКМ"] == "ЭЭ", "тип_ЛКМ"] = 3
df15.loc[df15["тип_ЛКМ"] == "МА", "тип_ЛКМ"] = 4
df15.loc[df15["тип_ЛКМ"] == "ПВ", "тип_ЛКМ"] = 11


# In[351]:


df15 = df15.drop_duplicates(subset=["code", "subnode_id"])
df15


# <h1>Общие условия эксплуатации</h1>

# In[352]:


df_op_cond = df15.loc[:, ["id", "коэф_надежн", "агрессивность", "t_эксплуат", "зона_эксплуат", "группа_газов", "материал", "тип_ЛКМ", "вп_болты"]]
df_op_cond


# In[353]:


df_op_cond.isna().sum()


# In[354]:


# Median fillna
df_op_cond["t_эксплуат"] = df_op_cond["t_эксплуат"].fillna(-34)


# In[355]:


df_op_cond.isna().sum()


# In[356]:


df_op_cond["агрессивность"].value_counts()


# In[357]:


df_op_cond = df_op_cond.rename(columns={"id": "mark_id",
                            "коэф_надежн": "safety_coeff",
                            "t_эксплуат": "temperature",
                            "зона_эксплуат": "operating_area_id",
                            "группа_газов": "gas_group_id",
                            "агрессивность": "env_aggressiveness_id",
                            "материал": "construction_material_id",
                            "тип_ЛКМ": "paintwork_type_id",
                            "вп_болты": "high_tensile_bolts_type_id"
                       })
df_op_cond


# <h1>Postgres</h1>

# In[268]:


cursor.execute(open("sql/y_mark_operating_conditions.sql", "r").read())
conn.commit()


# In[358]:


execute_values(conn, df_op_cond, "mark_operating_conditions")


# <h1>Mark continuation</h1>

# In[149]:


list(df15.columns)


# <h2>Не вижу главного строителя. Заменю н_контр на главного строителя</h2>

# In[289]:


df15 = df15.rename(columns={"код_отд": "department_id",
                            "подп1": "signed1_id",
                            "подп2": "signed2_id",
                            "гл_спец": "chief_specialist_id",
                            "рук_гр": "group_leader_id",
                            "н_контр": "main_builder_id",
                            "дата_выд": "issued_date",
                            "кол_томов": "num_of_volumes",
                            "дата_ред": "edited_date",
                            "коэф_надежн": "safety_coeff",
                            "t_эксплуат": "operating_temp",
                            "зона_эксплуат": "operating_zone",
                            "группа_газов": "gas_group",
                            "агрессивность": "aggressiveness",
                            "материал": "material",
                            "тип_ЛКМ": "paintwork_type",
                            "прим": "note",
                            "категория_пр": "fire_hazard_category_id",
                            "вп_болты": "high_tensile_bolts",
                            "п_транспорт": "p_transport",
                            "п_площадки": "p_site",
                            "10ХСНД": "xcnd",
                            "ТекстЗдСм": "text_3d_estimate",
                            "ДопОбъемы": "add_volumes",
                            "ВесПоВМП": "vmp_weight",
                            "ВыпЗдСм": "impl_3d_estimate",
                            "m_p": "name"
                       })
df15


# In[291]:


df15["issued_date"] = df15["issued_date"].astype(object).where(df15["issued_date"].notnull(), None)
df15["edited_date"] = df15["edited_date"].astype(object).where(df15["edited_date"].notnull(), None)
df15 = df15.where(pd.notnull(df15), None)


# In[292]:


df15["chief_specialist_id"] = df15["chief_specialist_id"].replace(0, None)
df15["group_leader_id"] = df15["group_leader_id"].replace(0, None)
df15["main_builder_id"] = df15["main_builder_id"].replace(0, None)


# <h1>Postgres</h1>

# In[293]:


cursor.execute(open("sql/7.sql", "r").read())
conn.commit()


# In[294]:


df15.iloc[0, :].values


# In[295]:


df15.columns.tolist()


# In[296]:


df16 = df15.reset_index(drop=True)
pd.DataFrame(df16.iloc[0, :].values.reshape(1, -1), columns=df16.columns.tolist())


# In[297]:


execute_values(conn, df16, "marks")


# <h1>Mark ids to csv</h1>

# In[298]:


df16["id"].to_csv("mark_ids.csv")


# In[ ]:





# In[ ]:




