import { ROUTES } from "@/shared/config/routes";
import NavBar from "@/shared/ui/nav-bar/NavBar";
import { ILinkInfo } from "@/shared/ui/nav-bar/NavBar.props";

export default function DepartmentsPage() {

      const links: ILinkInfo[] = [
        {
          name: 'Home',
          path: ROUTES.home,
        },

        {
        name: 'Locations',
        path: ROUTES.locations
      },
      {
        name: 'Positions',
        path: ROUTES.positions
      },]

  return (
    <div className="mx-auto pt-5">
      <h1>Departments</h1>
      
    <NavBar links={links}/>
    </div>

  )
}