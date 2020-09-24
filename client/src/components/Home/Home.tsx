import React from 'react'
import { Link } from 'react-router-dom'
import './Home.css'

const Home = () => {
    return (
        <div className="flex-v">
            <Link to="/mark-select" className="home-link">
                Выбрать марку
            </Link>
            <Link to="/mark-data" className="home-link">
                Данные марки
            </Link>
            <Link to="/mark-approval" className="home-link">
                Согласования
            </Link>
            <Link to="/specifications" className="home-link">
                Выпуски спецификаций
            </Link>
            <Link to="/sheets" className="home-link">
                Листы
            </Link>
            <Link to="/documents" className="home-link">
                Прилагаемые документы
            </Link>
            <Link to="/exploitation" className="home-link">
                Условия эксплуатации
            </Link>
        </div>
    )
}

export default Home
