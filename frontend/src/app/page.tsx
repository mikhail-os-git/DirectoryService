"use client";
import { Button } from "@/shared/ui/button";
import { useState } from "react";

export default function Home() {
  const [isVisible, setIsVisible] = useState(false);

  return (
    <div>
      {isVisible && <h1>Welcome in Directory Service!</h1>}
      <Button variant="outline" onClick={() => setIsVisible((v) => !v)}>
        {isVisible ? "Hide" : "Show"} greet
      </Button>
    </div>
  );
}