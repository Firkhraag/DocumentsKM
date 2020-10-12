import React from 'react'
import { Link } from 'react-router-dom'
import { useMark } from '../../store/MarkStore'
import './Home.css'

const Home = () => {

    const mark = useMark()

    let tileClassName = "home-link container white-bg text-centered"
    if (mark == null) {
        tileClassName += ' disabled'
    }

    return (
        <div className="home-cnt">
            <Link to="/mark-select" className="home-link container white-bg text-centered">
                Выбрать / создать марку
            </Link>
            <Link to="/mark-data" className={tileClassName}>
                Данные марки
            </Link>
            <Link to="/mark-approval" className={tileClassName}>
                Согласования
            </Link>
            <Link to="/specifications" className={tileClassName}>
                Выпуски спецификаций
            </Link>
            <Link to="/sheets" className={tileClassName}>
                Листы
            </Link>
            <Link to="/documents" className={tileClassName}>
                Прилагаемые документы
            </Link>
            <Link to="/exploitation" className={tileClassName}>
                Условия эксплуатации
            </Link>
        </div>
    )
}

export default Home
