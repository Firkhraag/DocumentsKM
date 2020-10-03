import React from 'react'
import { Link } from 'react-router-dom'
import './Home.css'

const Home = () => {
    return (
        <div className="home-cnt">
            <Link to="/mark-select" className="home-link container white-bg text-centered">
                Выбрать / добавить марку
            </Link>
            <Link to="/mark-data" className="home-link container white-bg text-centered">
                Данные марки
            </Link>
            <Link to="/mark-approval" className="home-link container white-bg text-centered">
                Согласования
            </Link>
            <Link to="/specifications" className="home-link container white-bg text-centered">
                Выпуски спецификаций
            </Link>
            <Link to="/sheets" className="home-link container white-bg text-centered">
                Листы
            </Link>
            <Link to="/documents" className="home-link container white-bg text-centered">
                Прилагаемые документы
            </Link>
            <Link to="/exploitation" className="home-link container white-bg text-centered">
                Условия эксплуатации
            </Link>
        </div>
    )
}

export default Home
