import React, { useState, useEffect } from 'react'
import './BrandData.css'

type BrandDataProps = {
	isCreateModeInitially: boolean
}

const BrandData = ({ isCreateModeInitially }: BrandDataProps) => {

    const [isCreateMode, setIsCreateMode] = useState<boolean>(isCreateModeInitially)
    const [brand, setBrand] = useState<string>(null)

    const [series, setSeries] = useState<Array<string>>([])
    const [nodes, setNodes] = useState<Array<string>>([])
    const [subnodes, setSubnodes] = useState<Array<string>>([])
    const [brands, setBrands] = useState<Array<string>>([])

    useEffect(() => {
        const seriesFetched: Array<string> = ['c', '2', '3']
        setSeries(seriesFetched)
    }, [])

    const onSeriesSelect = (event: React.FormEvent<HTMLSelectElement>) => {
        if (event.currentTarget.value !== "") {
            const nodes: Array<string> = ['ff', 'gg', 'c3']
            setNodes(nodes)
        }
    }

    const onNodeSelect = (event: React.FormEvent<HTMLSelectElement>) => {
        if (event.currentTarget.value !== "") {
            const subnodes: Array<string> = ['1', '2', '3']
            setSubnodes(subnodes)
        }
    }

    const onSubnodeSelect = (event: React.FormEvent<HTMLSelectElement>) => {
        if (event.currentTarget.value !== "") {
            const brands: Array<string> = ['1', '2', '3']
            setBrands(brands)
        }
    }

    return (
        <div>
            <h1 className="text-centered brand-data-title">Данные по марке</h1>
        <div className="tabs">
            <input type="radio" name="tab-btn" id="tab-btn-1" value="" onChange={() => setIsCreateMode(false)} checked={isCreateMode ? false : true} />
            <label htmlFor="tab-btn-1">Выбрать</label>
            <input type="radio" name="tab-btn" id="tab-btn-2" value="" onChange={() => setIsCreateMode(true)} checked={isCreateMode ? true : false} />
            <label htmlFor="tab-btn-2">Добавить</label>

            <div className="brand-data">
                { isCreateMode ? null : <div className="flex-cent-v mrg-bot">
                    <p>Последние марки</p>
                    <select className="input-width-3 mrg-left mrg-right border input-border-radius input-padding">
                        <option value=""></option>
                        <option>Пункт 1</option>
                        <option>Пункт 2</option>
                    </select>
                </div> }
                <div className="flex-cent-v mrg-bot">
                    <p>Проект</p>
                    <select onChange={onSeriesSelect} className="input-width-1 mrg-left mrg-right border input-border-radius input-padding">
                        <option key={-1}></option>
                        {series.map((x,y) => <option key={y}>{x}</option>)}
                        {/* <option value="" hidden>Серия</option> */}
                        {/* <option>Пункт 1</option> */}
                        {/* <option>Пункт 2</option> */}
                    </select>
                    <p>.</p>
                    <select onChange={onNodeSelect} className="input-width-1 mrg-left mrg-right border input-border-radius input-padding">
                        <option key={-1}></option>
                        {nodes.map((x,y) => <option key={y}>{x}</option>)}
                        {/* <option value="" hidden>Узел</option>
                        <option>Пункт 1</option>
                        <option>Пункт 2</option> */}
                    </select>
                    <p>.</p>
                    <select onChange={onSubnodeSelect} className="input-width-1 mrg-left mrg-right border input-border-radius input-padding">
                        <option key={-1}></option>
                        {subnodes.map((x,y) => <option key={y}>{x}</option>)}
                        {/* <option value="" hidden>Подузел</option>
                        <option>Пункт 1</option>
                        <option>Пункт 2</option> */}
                    </select>
                    <p>-</p>
                    <select className="input-width-1 mrg-left mrg-right border input-border-radius input-padding">
                        <option key={-1}></option>
                        {brands.map((x,y) => <option key={y}>{x}</option>)}
                        {/* <option value="" hidden>Марка</option>
                        <option>Пункт 1</option>
                        <option>Пункт 2</option> */}
                    </select>
                </div>
                <div className="space-between-cent-v mrg-bot">
                    <div className="flex">
                        <p>Отдел</p>
                        <select className="input-width-2 mrg-left mrg-right border input-border-radius input-padding">
                            <option>Пункт 1</option>
                            <option>Пункт 2</option>
                        </select>
                    </div>
                    <div className="flex">
                        <p>Шифр марки</p>
                        <select className="input-width-1 mrg-left mrg-right border input-border-radius input-padding">
                            <option>Пункт 1</option>
                            <option>Пункт 2</option>
                        </select>
                    </div>
                </div>
                <div className="mrg-bot">
                    Таблица
                </div>
                <div className="mrg-bot flex-cent-v">
                    <label htmlFor="agreements">Согласования</label>
                    <input className="mrg-left mrg-right checkbox" type="checkbox" id="agreements" name="agreements" />
                </div>
                <button className="input-border-radius pointer">Добавить новую марку</button>
            </div>
        </div>
        </div>
    )
}

export default BrandData