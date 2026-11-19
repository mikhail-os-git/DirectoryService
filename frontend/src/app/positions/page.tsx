import { ROUTES } from "@/shared/config/routes";
import NavBar from "@/shared/ui/nav-bar/NavBar";
import { ILinkInfo } from "@/shared/ui/nav-bar/NavBar.props";

export default function PositionsPage() {
    
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
          name: 'Locations',
          path: ROUTES.locations
        }]

  return (
    <div className="mx-auto pt-5">
      <h1> Positions </h1>
    <NavBar links={links}/>
    </div>
  )
}