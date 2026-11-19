import Link from "next/link"
import styles from './NavBar.module.css';
import NavBarProps from "./NavBar.props";
import { cn } from "@/shared/lib/utils";




export default function NavBar({links, ...props} : NavBarProps) {
  
  const items: React.ReactNode[] = links.map(link => (
  <Link key={link.path} href={link.path} className={cn(styles['btn'])}>
    {link.name}
  </Link>))
  
  return (
    <nav {...props} className={cn(styles['nav'])}>
      <div className={cn(styles['container'])}>
          {items}
      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 400 60" height={60} width={400} overflow="visible" className={cn(styles['outline'])}>
      <rect strokeWidth={5} fill="transparent" height={60} width={400} y={0} x={0} pathLength={100} className={cn(styles['rect'])} />
      </svg>
      </div>
    </nav>
  )
}