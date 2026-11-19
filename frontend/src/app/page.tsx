"use client";
import { ROUTES } from "@/shared/config/routes";
import NavBar from "@/shared/ui/nav-bar/NavBar";
import { ILinkInfo } from "@/shared/ui/nav-bar/NavBar.props";

export default function Home() {

    const links: ILinkInfo[] = [
      {
      name: 'Locations',
      path: ROUTES.locations
    },
    {
      name: 'Positions',
      path: ROUTES.positions
    },
          {
      name: 'Departments',
      path: ROUTES.departments
    },

  ] 

  return (
    <main className="main mx-auto pt-5">
      <h1>Home</h1>
      <NavBar links={links}/>
    </main>
  );
}