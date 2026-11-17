"use client";
import { ROUTES } from "@/shared/config/routes";
import NavBar from "@/shared/ui/nav-bar/NavBar";
import { ILinkInfo } from "@/shared/ui/nav-bar/NavBar.props";
import { useState } from "react"; 
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



    <main className="main mx-3">
      <NavBar links={links}/>
    </main>
  );
}