import { ROUTES } from "@/shared/config/routes"
import NavBar from "@/shared/ui/nav-bar/NavBar"
import { ILinkInfo } from "@/shared/ui/nav-bar/NavBar.props"

export default function LocationsPage(){
      
  const links: ILinkInfo[] = [
          {
            name: 'Home',
            path: ROUTES.home,
          },

          {
          name: 'Departments',
          path: ROUTES.departments
        },
        {
          name: 'Positions',
          path: ROUTES.positions
        }]

  return (
    <div className="mx-auto pt-5">
      <h1>Locations</h1>
    <NavBar links={links}/>
    </div>

    
  )
}